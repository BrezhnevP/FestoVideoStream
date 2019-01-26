using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;

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

        // GET: api/stream/dash/5
        [HttpGet("{type}/{id}")]
        public IActionResult GetManifestPath([FromRoute] string type, [FromRoute]Guid id)
        {
            var streamPath = _configuration.GetValue<string>("HttpServerPath");
            var manifestPath = $"{streamPath}{type}/{id}.{(type == "dash" ? "mpd" : "m3u8")}";
            
            if (UrlExists(manifestPath))
                return Ok(manifestPath);
            return NotFound();
        }

        private static bool UrlExists(string url)
        {
            bool result = true;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200;
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}