using FestoVideoStream.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

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

        // GET: api/stream/dash/1
        [HttpGet("dash/{id}")]
        public IActionResult GetManifestPath([FromRoute] int id)
        {
            var manifestPath = _service.GetDeviceDashManifest(id);
            if (manifestPath != null)
                return Ok(manifestPath);

            return NotFound();
        }

        // GET: api/stream/1/frames/5
        [HttpGet("{id}/frames/{count}")]
        public IActionResult GetFrames([FromRoute] int id, [FromRoute] int count)
        {
            var result = CreateFrames(id, count);
            if (result != Ok())
                return result;

            return Ok(_service.GetFilesUri(id, count));
        }

        private IActionResult CreateFrames(int id, int count)
        {
            var rtmp = _service.GetDeviceRtmpPath(id);
            if (rtmp == null)
                return NotFound();

            const string directory = "/tmp/frames";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments =
                        $"ffmpeg -y -i {rtmp} -vframes {count} {directory}/{_service.GetFramesFilePattern(id)}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            if (!string.IsNullOrEmpty(process.StandardError.ReadToEnd()))
                return BadRequest();

            process.WaitForExit();

            return Ok();
        }
    }
}