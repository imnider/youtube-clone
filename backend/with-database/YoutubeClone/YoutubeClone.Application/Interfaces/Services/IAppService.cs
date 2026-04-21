using YoutubeClone.Application.Models.DTOs;
using YoutubeClone.Application.Models.Responses;

namespace YoutubeClone.Application.Interfaces.Services
{
    public interface IAppService
    {
        Task<GenericResponse<AppInfoDto>> Info();
    }
}
