using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace FestoVideoStream.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// The stream controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        /// <summary>
        /// The stream service.
        /// </summary>
        private readonly StreamService _streamService;

        /// <summary>
        /// The path service.
        /// </summary>
        private readonly PathService _pathService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController"/> class.
        /// </summary>
        /// <param name="streamService">
        /// The stream service.
        /// </param>
        /// <param name="pathService">
        /// The path service.
        /// </param>
        public StreamController(StreamService streamService, PathService pathService)
        {
            this._pathService = pathService;
            this._streamService = streamService;
        }

        /// GET: api/stream/1/dash
        /// <summary>
        /// Get MPEG-DASH manifest url.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet("{id}/dash")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetManifestUrl([FromRoute] Guid id)
        {
            var manifestPath = this._pathService.GetDeviceDashManifest(id);
            if (!ConnectionService.UrlExists(manifestPath).Result)
            {
                return NotFound();
            }

            return Ok(manifestPath);
        }

        /// GET: api/stream/1/rtmp
        /// <summary>
        /// Get RTMP stream url.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet("{id}/rtmp/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRtmpStreamUrl([FromRoute] Guid id)
        {
            var rtmpPath = this._pathService.GetDeviceRtmpPath(id);

            return Ok(rtmpPath);
        }

        /// GET: api/stream/1/frames/5
        /// <summary>
        /// Get frames from video stream.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [HttpGet("{id}/frames/{count}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFrames([FromRoute] Guid id, [FromRoute] int count)
        {
            var result = this.CreateFrames(id, count);

            return result != this.Ok() ? result : this.Ok(this._streamService.GetFilesUri(id, count));
        }

        /// <summary>
        /// Create frames.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        private IActionResult CreateFrames(Guid id, int count)
        {
            var rtmp = this._pathService.GetDeviceRtmpPath(id);
            if (rtmp == null)
            {
                return NotFound();
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments =
                        $"ffmpeg -y -i {rtmp} -vframes {count} {this._pathService.FramesDirectory}/{this._streamService.GetFramesFilePattern(id)}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            if (!string.IsNullOrEmpty(process.StandardError.ReadToEnd()))
            {
                return BadRequest();
            }
            process.WaitForExit();

            return Ok();
        }
    }
}