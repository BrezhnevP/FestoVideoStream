using FestoVideoStream.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private readonly HttpVideoStreamService _streamService;

        public StreamController(HttpVideoStreamService streamService)
        {
            this._streamService = streamService;
        }

        [HttpGet]
        public async Task<FileStreamResult> GetVideoStream()
        {
            var stream = await _streamService.GetVideoStream();
            return new FileStreamResult(stream, "application/vnd.apple.mpegurl");
        }

        /*
        // GET: api/Stream
        [HttpGet]
        public HttpResponseMessage GetPushVideoStream()
        {
            return new HttpResponseMessage
            {
                Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)_streamService.WriteContentToStream)
            }; ;
        }
        */
    }
}