using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        public async Task<GenericResponse<UserDto>> Create(CreateUserRequest model)
        {
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
                CreatedAt = DateTimeHelper.UtcNow()
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

            return ResponseHelper.Create(true);
        }

        public async Task<GenericResponse<List<UserDto>>> GetAll(FilterUserRequest model)
        {
            var queryable = uow.userRepository.Queryable();

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
            var users = queryable.Skip(model.Offset).Take(model.Limit).ToList();

            // Mapear usuarios
            List<UserDto> mapped = [];
            foreach (var user in users)
            {
                mapped.Add(Map(user));
            }

            return ResponseHelper.Create(mapped);
        }

        public async Task<GenericResponse<UserDto>> GetById(Guid id)
        {
            var user = await GetUser(id);
            return ResponseHelper.Create(Map(user));
        }

        private async Task<UserAccount> GetUser(Guid id)
        {
            return await uow.userRepository.Get(id)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);
        }

        private static UserDto Map(UserAccount user)
        {
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

            };
        }

        public async Task<GenericResponse<UserDto>> Update(Guid id, UpdateUserRequest model)
        {
            var user = await GetUser(id);

            user.UserName = model.UserName ?? user.UserName;
            user.DisplayName = model.DisplayName ?? user.DisplayName;
            user.Email = model.Email ?? user.Email;
            user.Birthday = model.Birthday ?? user.Birthday;
            user.Location = model.Location ?? user.Location;

            user.UpdatedAt = DateTimeHelper.UtcNow();

            var update = await uow.userRepository.Update(user);

            await uow.SaveChangesAsync();

            return ResponseHelper.Create(Map(user));
        }

        public async Task CreateFirstUser()
        {
            Console.WriteLine("Entrando a CreateFirstUser");

            var count = await uow.userRepository.Queryable().CountAsync();
            Console.WriteLine($"Usuarios activos: {count}");

            var hasCreated = await uow.userRepository.HasCreated();
            Console.WriteLine($"HasCreated: {hasCreated}");

            //var hasCreated = await repository.HasCreated();
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

            await uow.userRepository.Create(new UserAccount
            {
                UserName = userName,
                DisplayName = displayName,
                Location = Location,
                Email = email,
                Password = Hasher.HashPassword(password)
            });

            await uow.SaveChangesAsync();
        }

        public async Task<GenericResponse<UserDto>> Me(Claim claim)
        {
            var executor = await GetExecutor(claim.Value);
            return ResponseHelper.Create(Map(executor));
        }

        // METODOS PRIVADOS
        private async Task<UserAccount> GetExecutor(string value)
        {
            var uuid = Guid.Parse(value);
            return await uow.userRepository.Get(uuid)
                ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXIST);
        }
    }
}