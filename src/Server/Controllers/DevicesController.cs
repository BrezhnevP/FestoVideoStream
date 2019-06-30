using AutoMapper;
using FestoVideoStream.Models.Dto;
using FestoVideoStream.Models.Entities;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using FestoVideoStream.Extensions;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private const int PageSize = 5;

        private readonly DevicesService devicesService;
        private readonly ILogger<DevicesController> logger;

        public DevicesController(DevicesService devicesService, ILogger<DevicesController> logger)
        {
            this.devicesService = devicesService;
            this.logger = logger;
        }

        // GET: api/Devices
        [HttpGet("{page?}/{sortBy?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDevices([FromQuery] int? page)
        {
            var devices = await this.devicesService.GetDevices();
            if (page != null)
            {
                devices = devices.Skip(((int)page - 1) * PageSize).Take(PageSize);
            }

            return Ok(devices.Select(d => d.ToDto()));
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDevice([FromRoute] Guid id)
        {
            var device = await this.devicesService.GetDevice(id);

            if (device == null)
            {
                logger.LogWarning($"Cannot find device with id - {id}");
                return NotFound();
            }
            logger.LogTrace($"Getting device {id}");

            return Ok(device.ToDetailsDto());
        }

        // PUT: api/Devices/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchDevice([FromRoute] Guid id, [FromBody] DeviceDetailsDto deviceDetailsDto)
        {
            if (id != deviceDetailsDto.Id)
            {
                return BadRequest();
            }

            var device = deviceDetailsDto.ToEntity();
            if (!TryValidateModel(device))
            {
                return BadRequest();
            }

            var result = await this.devicesService.UpdateDevice(id, device);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        // POST: api/Devices
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostDevice([FromBody] DeviceDetailsDto deviceToCreate)
        {
            var device = deviceToCreate.ToEntity();
            if (!TryValidateModel(device))
            {
                return BadRequest();
            }
            var createdDevice = await this.devicesService.CreateDevice(device);

            return CreatedAtAction("GetDevice", new { createdDevice.Id }, createdDevice);
        }

        // DELETE: api/Devices/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await this.devicesService.DeleteDevice(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}