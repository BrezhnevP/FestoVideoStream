﻿using AutoMapper;
using FestoVideoStream.Dto;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using FestoVideoStream.Entities;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly DevicesService _service;
        private readonly IMapper _mapper;

        public DevicesController(DevicesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Devices
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetDevices()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var devices = _service.GetDevices().Select(d => _mapper.Map<DeviceDto>(d));

            return Ok(devices);
        }

        // GET: api/Devices/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDevice([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var device = _mapper.Map<DeviceDetailsDto>(await _service.GetDevice(id));

            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchDevice([FromRoute] Guid id, [FromBody] DeviceDetailsDto device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateDevice(id, _mapper.Map<Device>(device));

            if (result == null)
            {
                BadRequest();
            }

            return Ok(result);
        }

        // POST: api/Devices
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostDevice([FromBody] DeviceDetailsDto deviceToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdDevice = await _service.CreateDevice(_mapper.Map<Device>(deviceToCreate));

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
            var result = await _service.DeleteDevice(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}