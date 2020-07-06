using Microsoft.AspNetCore.Mvc;

namespace WaterController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("ping")]
        public object Ping()
        {
            return Ok("pong");
        }
    }
}