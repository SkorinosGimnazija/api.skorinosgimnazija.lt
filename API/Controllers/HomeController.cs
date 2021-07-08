namespace API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("👌");
        }
    }
}