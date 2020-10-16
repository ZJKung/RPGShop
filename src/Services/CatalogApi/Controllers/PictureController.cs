using System.IO;
using System.Threading.Tasks;
using CatalogApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public PictureController(IWebHostEnvironment env)
        {
            _env=env;
        }
        [HttpGet]
        [Route("{fileName}")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot+"/Pictures/",fileName);
            var buffer = await System.IO.File.ReadAllBytesAsync(path);
            return File(buffer,"image/png");
        }



    }
}