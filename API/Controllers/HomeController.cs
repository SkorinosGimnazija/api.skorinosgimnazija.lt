namespace API.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        return Ok("👌");
    }
}