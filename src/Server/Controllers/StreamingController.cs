using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        // GET: api/Stream/GUID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHLSManifestPath([FromRoute]Guid id)
        {
            return Ok(id.ToString());
        }
    }
}