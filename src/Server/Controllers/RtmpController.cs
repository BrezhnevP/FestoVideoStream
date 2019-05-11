using FestoVideoStream.Models;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RtmpController : ControllerBase
    {
        private readonly DevicesService _devicesService;
        private readonly ILogger<RtmpController> logger;

        public RtmpController(DevicesService devicesService, ILogger<RtmpController> logger)
        {
            _devicesService = devicesService;
            this.logger = logger;
        }

        [HttpPost("publish")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Publish([FromForm] RtmpServerNotification notification)
        {
            if (notification.Name == null || !Guid.TryParse(notification.Name, out var id))
            {
                logger.LogWarning($"Incorrect identifier. Cannot publish.");
                return BadRequest();
            }

            await this.UpdateStream(id, true);

            return Ok();
        }

        [HttpPost("done")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Done([FromForm] RtmpServerNotification notification)
        {
            if (notification.Name == null || !Guid.TryParse(notification.Name, out var id))
            {
                logger.LogWarning($"Incorrect identifier. Cannot done.");
                return BadRequest();
            }

            await this.UpdateStream(id, false);

            return Ok();
        }

        [HttpPost("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Update([FromForm] RtmpServerNotification notification)
        {
            if (notification.Name == null || !Guid.TryParse(notification.Name, out var id))
            {
                logger.LogWarning($"Incorrect identifier. Cannot update.");
                return BadRequest();
            }

            await this.UpdateStream(id, true);

            return Ok();
        }

        private async Task UpdateStream(Guid id, bool currentStreamStatus)
        {
            var device = await _devicesService.GetDevice(id);
            if (device != null)
            {
                if (device.StreamStatus != currentStreamStatus)
                {
                    if (!device.StreamStatus && currentStreamStatus)
                    {
                        device.StreamStatus = true;
                        device.LastStreamStartDate = DateTime.UtcNow;
                        device.LastStreamEndDate = null;
                    }
                    else if (device.StreamStatus == true && currentStreamStatus == false)
                    {
                        device.StreamStatus = false;
                        device.LastStreamEndDate = DateTime.UtcNow;
                    }
                    logger.LogInformation($"Updating device ({id}) with stream status - {(currentStreamStatus ? "online" : "offline")}");
                    if (await _devicesService.UpdateDevice(device) != null)
                        logger.LogInformation($"Device ({id}) is updated");
                }
            }
            else
            {
                logger.LogWarning($"Cannot find device with id - {id}");
            }
        }
    }
}           