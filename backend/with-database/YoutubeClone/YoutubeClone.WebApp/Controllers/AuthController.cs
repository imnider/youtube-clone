using Microsoft.AspNetCore.Mvc;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.Requests.Auth;

namespace YoutubeClone.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthServices service) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAuthRequest model)
        {
            var srv = await service.Login(model);
            return Ok(srv);
        }

        [HttpPost("renew")]
        public async Task<IActionResult> Renew([FromBody] RenewAuthRequest model)
        {
            var srv = await service.Renew(model);
            return Ok(srv);
        }
    }
}
