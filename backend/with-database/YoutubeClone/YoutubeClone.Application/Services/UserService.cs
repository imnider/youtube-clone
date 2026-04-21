using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Claims;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Requests.Users;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Shared;
using YoutubeClone.Shared.Constants;
using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Services
{
    public class UserService(IUnitOfWork uow, IConfiguration configuration, SMTP smtp, IEmailTemplateService emailTemplateService) : IUserService
    {
        public async Task<GenericResponse<UserDto>> Create(CreateUserRequest model, Claim? claim)
        {
            Role? roleToAssign = null;
            UserAccount? executor = null;

            if (claim is not null)
            {
                executor = await GetExecutor(claim.Value);
                if (!model.RoleId.HasValue || model.RoleId.HasValue && model.RoleId.Value == Guid.Empty)
                {
                    throw new NotFoundException(ValidationConstants.IsEmpty("RoleId"));
                }
                await ValidateEmailIfExists(model.Email);
                roleToAssign = await ValidateRole(executor, model.RoleId.Value);
            }
            else
            {
                roleToAssign = await uow.roleRepository.Get(x => x.Name == RoleConstants.CreadorContenido);
            }

            if (roleToAssign is null)
            {
                throw new BadRequestException("Imposible obtener el rol para asignarle al usuario");
            }

            var password = Generate.RandomText(32);

            var create = await uow.userRepository.Create(new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = model.UserName.ToLower(),
                DisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.DisplayName.ToLower()),
                Email = model.Email.ToLower(),
                Birthday = model.Birthday,
                Location = model.Location,
                Password = Hasher.HashPassword(password),
                CreatedAt = DateTimeHelper.UtcNow(),
                UserAccountRoles = [new UserAccountRole {
                    RoleId = roleToAssign.RoleId,
                    AssignedBy = executor?.UserId,
                    AssignedAt = DateTimeHelper.UtcNow()
                }]
            });

            var template = await emailTemplateService.Get(EmailTemplateNameConstants.USER_REGISTER, new Dictionary<string, string>
            {
                { "password", password }
            });
            await smtp.Send(model.Email, template.Subject, template.Body);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create(Map(create), [], "Usuario creado correctamente.");
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var user = await GetUser(id);

            user.DeletedAt = DateTimeHelper.UtcNow();

            await uow.userRepository.Update(user);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create(true);
        }

        public async Task<GenericResponse<List<UserDto>>> GetAll(FilterUserRequest model)
        {
            var queryable = uow.userRepository.Queryable();

            queryable = queryable.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                queryable = queryable.Where(x => x.UserName.Contains(model.UserName ?? ""));
            }
            if (!string.IsNullOrWhiteSpace(model.DisplayName))
            {
                queryable = queryable.Where(x => x.DisplayName.Contains(model.DisplayName ?? ""));
            }
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                queryable = queryable.Where(x => x.Email.Contains(model.Email ?? ""));
            }
            // validar birthday
            if (!string.IsNullOrWhiteSpace(model.Location))
            {
                queryable = queryable.Where(x => x.Location.Contains(model.Location ?? ""));
            }

            // Paginación y consulta
            var users = queryable
                .Include(user => user.UserAccountRoles)
                .ThenInclude(userRole => userRole.Role)
                .AsQueryable()
                .Skip(model.Offset)
                .Take(model.Limit)
                .Select(user => Map(user))
                .ToList();

            return ResponseHelper.Create(users);
        }

        public async Task<GenericResponse<UserDto>> GetById(Guid id)
        {
            var user = await GetUser(id);
            return ResponseHelper.Create(Map(user));
        }

        public async Task<GenericResponse<UserDto>> Update(Guid id, UpdateUserRequest model, Claim? claim)
        {
            var executor = await GetExecutor(claim.Value);
            var user = await GetUser(id);

            user.UserName = model.UserName ?? user.UserName;
            user.DisplayName = model.DisplayName ?? user.DisplayName;
            user.Email = model.Email ?? user.Email;
            user.Birthday = model.Birthday ?? user.Birthday;
            user.Location = model.Location ?? user.Location;

            if (!string.IsNullOrWhiteSpace(model.Email) && user.Email != model.Email)
            {
                await ValidateEmailIfExists(model.Email);
                user.Email = model.Email;
            }

            if (model.RoleId.HasValue)
            {
                var roleToAssign = await ValidateRole(executor, model.RoleId.Value);

                await uow.userRepository.ClearRoles([.. user.UserAccountRoles]);

                user.UserAccountRoles.Add(new UserAccountRole
                {
                    RoleId = roleToAssign.RoleId,
                    AssignedBy = executor.UserId,
                    AssignedAt = DateTimeHelper.UtcNow()
                });
            }

            user.UpdatedAt = DateTimeHelper.UtcNow();

            var update = await uow.userRepository.Update(user);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create(Map(user));
        }

        public async Task CreateFirstUser()
        {
            var hasCreated = await uow.userRepository.HasCreated();

            if (hasCreated) return;

            var userName = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_USERNAME]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_USERNAME));

            var displayName = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_DISPLAYNAME]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_DISPLAYNAME));

            var Location = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_LOCATION]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_LOCATION));

            var email = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_EMAIL]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_EMAIL));

            var password = configuration[ConfigurationConstants.FIRST_APP_TIME_USER_PASSWORD]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.FIRST_APP_TIME_USER_PASSWORD));

            var adminRole = await uow.roleRepository.Get(x => x.Name == RoleConstants.Administrador)
                ?? throw new Exception(ResponseConstants.RoleNotFound(RoleConstants.Administrador));

            await uow.userRepository.Create(new UserAccount
            {
                UserName = userName,
                DisplayName = displayName,
                Location = Location,
                Email = email,
                Password = Hasher.HashPassword(password),
                UserAccountRoles = [new UserAccountRole {
                    RoleId = adminRole.RoleId,
                }]
            });

            await uow.SaveChangesAsync();
        }

        public async Task<GenericResponse<UserDto>> Me(Claim claim)
        {
            var executor = await GetExecutor(claim.Value);
            return ResponseHelper.Create(Map(executor));
        }

        // METODOS PRIVADOS
        private async Task<UserAccount> GetUser(Guid id)
        {
            return await uow.userRepository.Get(id)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);
        }

        private static UserDto Map(UserAccount user)
        {
            var role = user.UserAccountRoles.FirstOrDefault()?.Role;

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Birthday = user.Birthday,
                Location = user.Location,
                Password = user.Password,
                CreatedAt = user.CreatedAt,
                Role = role != null ? new RoleDto
                {
                    Id = role.RoleId,
                    Name = role.Name,
                    Description = role.Description
                } : null
            };
        }

        public async Task<UserAccount> GetExecutor(string value)
        {
            var uuid = Guid.Parse(value);
            return await uow.userRepository.Get(uuid)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);
        }

        private async Task ValidateEmailIfExists(string email)
        {
            if (await uow.userRepository.IfExists(x => x.Email == email))
            {
                throw new BadRequestException(ResponseConstants.USER_EMAIL_TAKED);
            }
        }

        private async Task<Role> ValidateRole(UserAccount executor, Guid roleId)
        {
            var roleToAssign = await uow.roleRepository.Get(roleId)
                ?? throw new NotFoundException(ResponseConstants.RoleNotFound(roleId));

            if (executor.UserAccountRoles.First().Role.Name == RoleConstants.CreadorContenido && roleToAssign.Name == RoleConstants.Administrador)
            {
                throw new BadRequestException(ResponseConstants.CANNOT_ASSIGN_THE_ROLE);
            }

            return roleToAssign;
        }
    }
}