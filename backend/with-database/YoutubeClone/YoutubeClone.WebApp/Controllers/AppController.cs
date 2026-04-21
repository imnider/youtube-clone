using Microsoft.AspNetCore.Mvc;
using YoutubeClone.Application.Interfaces.Services;

namespace YoutubeClone.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppController(IAppService appService) : ControllerBase
    {
        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            var srv = await appService.Info();
            return Ok(srv);
        }
    }
}
