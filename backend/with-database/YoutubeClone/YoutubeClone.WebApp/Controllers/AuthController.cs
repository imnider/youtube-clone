using Microsoft.AspNetCore.Mvc;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.Requests.Auth;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Application.Models.Responses.Auth;
using YoutubeClone.WebApp.Helpers;

namespace YoutubeClone.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices service) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<GenericResponse<LoginAuthResponse>> Login([FromBody] LoginAuthRequest model)
        {
            var rsp = await service.Login(model);
            return ResponseStatus.Created(HttpContext, rsp);
        }

        [HttpPost("renew")]
        public async Task<GenericResponse<LoginAuthResponse>> Renew([FromBody] RenewAuthRequest model)
        {
            var rsp = await service.Renew(model);
            return ResponseStatus.Created(HttpContext, rsp);
        }
    }
}
