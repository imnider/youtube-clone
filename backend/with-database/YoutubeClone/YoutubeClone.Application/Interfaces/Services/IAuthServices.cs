using YoutubeClone.Application.Models.Requests.Auth;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Application.Models.Responses.Auth;

namespace YoutubeClone.Application.Interfaces.Services
{
    public interface IAuthServices
    {
        Task<GenericResponse<LoginAuthResponse>> Login(LoginAuthRequest model);
        Task<GenericResponse<LoginAuthResponse>> Renew(RenewAuthRequest model);
    }
}
