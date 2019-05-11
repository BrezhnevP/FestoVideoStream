using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly StreamService streamService;

        /// <summary>
        /// The path service.
        /// </summary>
        private readonly PathService pathService;

        private readonly ILogger<StreamController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamController"/> class.
        /// </summary>
        /// <param name="streamService">
        /// The stream service.
        /// </param>
        /// <param name="pathService">
        /// The path service.
        /// </param>
        public StreamController(StreamService streamService, PathService pathService, ILogger<StreamController> logger)
        {
            this.pathService = pathService;
            this.logger = logger;
            this.streamService = streamService;
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
            var manifestPath = this.pathService.GetDeviceDashManifest(id);
            if (!ConnectionService.UrlExistsAsync(manifestPath).Result)
            {
                logger.LogWarning($"Cannot find MPEG-DASH manifest with id - {id}");
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
        [HttpGet("{id}/rtmp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRtmpStreamUrl([FromRoute] Guid id)
        {
            var rtmpPath = this.pathService.GetDeviceRtmpPath(id);

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
            return this.Ok(this.streamService.GetFilesUri(id, count));
            var result = this.CreateFrames(id, count);
            return result == true ?
                       this.Ok(this.streamService.GetFilesUri(id, count)) :
                       result == false ?
                           (IActionResult)this.BadRequest() :
                           this.NotFound();
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
        private bool? CreateFrames(Guid id, int count)
        {
            var rtmp = this.pathService.GetDeviceRtmpPath(id);
            if (rtmp == null)
            {
                return null;
            }

            var processInfo = new ProcessStartInfo(
                "sudo",
                $"ffmpeg -y -i {rtmp} -vframes {count} {this.pathService.FramesDirectory}/{StreamService.GetFramesFilePattern(id)}");
            using (var p = Process.Start(processInfo))
            {
                if (p != null)
                {
                    var strOutput = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                }
            }

            return true;
        }
    }
}