using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StreamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Stream/5
        [HttpGet("{id}")]
        public IActionResult GetManifestPath([FromRoute]Guid id)
        {
            var streamManifestPath = _configuration.GetValue<string>("ServerPath");

            if (System.IO.File.Exists($"{streamManifestPath}/{id}.m3u8"))
                return Ok($"{streamManifestPath}/{id}.m3u8");
            return NotFound();
        }
    }
}