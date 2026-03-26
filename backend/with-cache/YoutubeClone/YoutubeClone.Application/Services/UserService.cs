using System.Globalization;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Requests.Users;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Shared;
using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Services
{
    public class UserService(Cache<UserDto> cache) : IUserService
    {
        public GenericResponse<UserDto> Create(CreateUserRequest model)
        {
            var newUser = new UserDto
            {
                UserId = Guid.NewGuid(),
                UserName = model.UserName.ToLower(),
                DisplayName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.DisplayName.ToLower()),
                Email = model.Email.ToLower(),
                Birthday = model.Birthday,
                Country = model.Country,
                Password = model.Password,
                CreatedAt = DateTimeHelper.UtcNow()
            };

            // Validar si existe un usuario con el mismo UserName y si el Email ya está registrado
            bool existUserName = false;
            bool existEmail = false;
            var users = cache.Get();
            foreach (UserDto user in users)
            {
                if (user.UserName == newUser.UserName) existUserName = true;
                if (user.Email == newUser.Email) existEmail = true;
            }
            if (existUserName) return ResponsesHelper.Create(newUser, "Ya existe un usuario con ese username.");
            if (existEmail) return ResponsesHelper.Create(newUser, "El email ya está registado");

            // Validar que su edad sea mayor a 13
            int miniumAge = 13;
            if (!ParentalControlHelper.hasMinimumAge(model.Birthday, miniumAge))
            {
                return ResponsesHelper.Create(newUser, $"Debe ser mayor de {13} años.");
            }

            // Validar el contenido de la Password
            if (!PasswordHelper.isValid(model.Password))
            {
                return ResponsesHelper.Create(newUser, "La contraseña debe tener al menos una mayúscula y un caracter especial.");
            }

            // Agregar a caché y crear
            cache.Add(newUser.UserId.ToString(), newUser);
            return ResponsesHelper.Create(newUser, "Usuario creado correctamente.");
        }

        public GenericResponse<bool> Delete(Guid id)
        {
            var exist = cache.Get(id.ToString());
            if (exist is null)
            {
                return ResponsesHelper.Create(false, "Usuario no encontrado.");
            }
            cache.Delete(id.ToString());
            return ResponsesHelper.Create(true, "Usuario eliminado con éxito.");
        }

        public GenericResponse<List<UserDto>> GetAll(int limit, int offset)
        {
            var users = cache.Get();
            if (users.Count is 0)
            {
                return ResponsesHelper.Create(users, "No se encontraron usuarios.");
            }
            return ResponsesHelper.Create(users, "Se encontraron usuarios creados.");
        }

        public GenericResponse<UserDto?> GetById(Guid id)
        {
            var user = cache.Get(id.ToString());
            if (user is null)
            {
                return ResponsesHelper.Create(user, "Usuario no encontrado.");
            }
            return ResponsesHelper.Create(user, "Usuario encontrado con éxito.");
        }
    }
}