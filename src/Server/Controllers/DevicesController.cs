using FestoVideoStream.Dto;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _service;

        public DevicesController(DevicesService service)
        {
            _service = service;
        }

        // GET: api/Devices
        [HttpGet]
        public IActionResult GetDevices()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var devices = _service.GetDevices();

            return Ok(devices);
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = await _service.GetDevice(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchDevice([FromRoute] int id, [FromBody] DeviceDetailsDto device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateDevice(id, device);

            if (!result)
            {
                BadRequest();
            }

            return Ok();
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<IActionResult> PostDevice([FromBody] DeviceDetailsDto deviceToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdDevice = await _service.CreateDevice(deviceToCreate);

            return CreatedAtAction("GetDevice", new { createdDevice.Id }, createdDevice);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _service.DeleteDevice(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}