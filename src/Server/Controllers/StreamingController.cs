using FestoVideoStream.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StreamService _service;

        public StreamController(IConfiguration configuration, StreamService service)
        {
            _configuration = configuration;
            _service = service;
        }

        // GET: api/stream/dash/5
        [HttpGet("dash/{id}")]
        public IActionResult GetManifestPath([FromRoute]Guid id)
        {
            var manifestPath = _service.GetDashManifest(id);
            if (manifestPath != null)
                return Ok(manifestPath);
            
            return NotFound();
        }

        // GET: api/stream/dash/5
        [HttpGet("{id}/frames/{count}")]
        public IActionResult GetFrames([FromRoute]Guid id, [FromRoute] int count)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                const string directory = "/tmp/screenshots";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sh",
                        Arguments =
                            $"ffmpeg -y -i {_service.GetRtmpPath(id)} -vframes {count} {directory}/out_{id}_%03d.jpg",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.Start();
                if (!string.IsNullOrEmpty(process.StandardError.ReadToEnd()))
                    return NotFound();
                process.WaitForExit();

                var files = Directory.GetFiles(directory, $"out_{id}_*").Select(f => System.IO.File.Open(f, FileMode.Open));
                return File(files.First(), "image/jpg");
            }
            return NotFound();
        }
    }
}