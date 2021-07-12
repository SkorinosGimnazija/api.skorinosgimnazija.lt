namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("👌");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Test(List<IFormFile> images)
        {
            foreach (var formFile in images)
            {
                if (formFile.Length > 0)
                {
                    var dirPath = Path.Combine(_config["FILE_UPLOAD_PATH"], DateTime.Now.ToString("yyyy.MM.dd"));
                    var ext = Path.GetExtension(formFile.FileName);

                    Console.WriteLine(dirPath);

                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    await using var stream = System.IO.File.Create(dirPath + "/" + Path.GetRandomFileName() + ext);
                    await formFile.CopyToAsync(stream);
                }
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> TestDel()
        { 
            var dirPath = Path.Combine(_config["FILE_UPLOAD_PATH"], DateTime.Now.ToString("yyyy.MM.dd"));
            var files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                Console.WriteLine(file);
                System.IO.File.Delete(file);
            }
            
            return Ok();
        }
    }
}