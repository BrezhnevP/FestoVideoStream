using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;
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
        private readonly StreamService streamService;
        private readonly PathService _pathService;

        public StreamController(IConfiguration configuration, StreamService streamService, PathService pathService)
        {
            _configuration = configuration;
            this._pathService = pathService;
            this.streamService = streamService;
        }

        // GET: api/stream/1/dash
        [HttpGet("{id}/dash/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetManifestPath([FromRoute] Guid id)
        {
            var manifestPath = _pathService.GetDeviceDashManifest(id);
            if (_pathService.UrlExists(manifestPath).Result)
                return Ok(manifestPath);

            return NotFound();
        }

        // GET: api/stream/1/rtmp
        [HttpGet("{id}/rtmp/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRtmpStreamPath([FromRoute] Guid id)
        {
            var rtmpPath = _pathService.GetDeviceRtmpPath(id);

            return Ok(rtmpPath);
        }

        // GET: api/stream/1/frames/5
        [HttpGet("{id}/frames/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFrames([FromRoute] Guid id, [FromRoute] int count)
        {
            var result = CreateFrames(id, count);
            if (result != Ok())
                return result;

            return Ok(streamService.GetFilesUri(id, count));
        }

        private IActionResult CreateFrames(Guid id, int count)
        {
            var rtmp = _pathService.GetDeviceRtmpPath(id);
            if (rtmp == null)
                return NotFound();

            const string directory = "/tmp/frames";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments =
                        $"ffmpeg -y -i {rtmp} -vframes {count} {directory}/{streamService.GetFramesFilePattern(id)}",
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