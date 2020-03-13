using Microsoft.AspNetCore.Mvc;

namespace TweetBook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { name = "Regi" });
        }
    }
}
