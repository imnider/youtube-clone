using System.Security.Claims;
using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Requests.Users;
using YoutubeClone.Application.Models.Responses;

namespace YoutubeClone.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<GenericResponse<UserDto>> Me(Claim claim);
        public Task<GenericResponse<UserDto>> Create(CreateUserRequest model);
        public Task<GenericResponse<List<UserDto>>> GetAll(FilterUserRequest model);
        public Task<GenericResponse<UserDto>> GetById(Guid id);
        public Task<GenericResponse<UserDto>> Update(Guid id, UpdateUserRequest model);
        public Task<GenericResponse<bool>> Delete(Guid id);
        public Task CreateFirstUser();

    }
}
