using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Requests.Users;
using YoutubeClone.Application.Models.Responses;

namespace YoutubeClone.Application.Interfaces.Services
{
    public interface IUserService
    {
        public GenericResponse<UserDto> Create(CreateUserRequest model);
        public GenericResponse<List<UserDto>> GetAll(int limit, int offset);
        public GenericResponse<UserDto?> GetById(Guid id);
        public GenericResponse<bool> Delete(Guid id);

    }
}
