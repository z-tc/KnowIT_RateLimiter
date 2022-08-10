using Microsoft.AspNetCore.Mvc;

namespace KnowITRateLimiter.API.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
       [HttpGet]
        public IActionResult Get()
        {
            return Ok(typeof(Startup).Assembly.GetName().Version?.ToString());
        }
    }
}
