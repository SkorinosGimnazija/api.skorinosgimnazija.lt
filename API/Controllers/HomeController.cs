namespace API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return Ok("👌");
        }
    }
}