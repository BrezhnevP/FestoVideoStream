using Microsoft.AspNetCore.Mvc;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RtmpController : ControllerBase
    {
        [HttpPost("publish")]
        public IActionResult Publish()
        {
            //TODO
            return Ok();
        }

        [HttpPost("done")]
        public IActionResult Done()
        {
            //TODO
            return Ok();
        }

        [HttpPost("update")]
        public IActionResult Update()
        {
            //TODO
            return Ok();
        }
    }
}