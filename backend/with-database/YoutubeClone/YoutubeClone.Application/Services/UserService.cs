using System.Globalization;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Requests.Users;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Domain.Database.SqlServer.Entities;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Domain.Interfaces.Repositories;
using YoutubeClone.Shared.Constants;
using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Services
{
    public class UserService(IUserRepository reposity) : IUserService
    {
        public async Task<GenericResponse<UserDto>> Create(CreateUserRequest model)
        {
            var create = await reposity.Create(new UserAccount
            {
                UserId = Guid.NewGuid(),
                UserName = model.UserName.ToLower(),
                DisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.DisplayName.ToLower()),
                Email = model.Email.ToLower(),
                Birthday = model.Birthday,
                Location = model.Location,
                Password = model.Password,
                CreatedAt = DateTimeHelper.UtcNow()
            });
            return ResponsesHelper.Create(Map(create), "Usuario creado correctamente.");
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var user = await GetUser(id);

            user.DeletedAt = DateTimeHelper.UtcNow();

            await reposity.Update(user);

            return ResponsesHelper.Create(true);
        }

        public async Task<GenericResponse<List<UserDto>>> GetAll(FilterUserRequest model)
        {
            var queryable = reposity.Queryable();

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
            // hacer birthday
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

            return ResponsesHelper.Create(mapped);
        }

        public async Task<GenericResponse<UserDto>> GetById(Guid id)
        {
            var user = await GetUser(id);
            return ResponsesHelper.Create(Map(user));
        }

        private async Task<UserAccount> GetUser(Guid id)
        {
            return await reposity.Get(id)
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

            user.UpdatedAt = DateTimeHelper.UtcNow();

            var update = await reposity.Update(user);

            return ResponsesHelper.Create(Map(user));
        }
    }
}