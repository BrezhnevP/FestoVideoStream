using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace FestoVideoStream.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    /// <summary>
    /// The devices data controller.
    /// </summary>
    [Route("api/Devices")]
    public class DevicesDataController : Controller
    {
        private readonly IEnumerable<Device> _devices = new List<Device>
        {
            new Device{
                Id = new Guid("e6942940-f619-4a71-8794-c72896de2c01"),
                IpAddress = IPAddress.Any.ToString(),
                Name = $"Device ¹ 1"
            },
            new Device{
                Id = new Guid("67cccb87-467c-49f9-9b4f-79698e1edc83"),
                IpAddress = IPAddress.Any.ToString(),
                Name = $"Device ¹ 2"
            },
            new Device{
                Id = new Guid("c2c663bf-3ffc-4326-afd7-880d4b25f8fc"),
                IpAddress = IPAddress.Any.ToString(),
                Name = $"Device ¹ 3"
            },
        };

        [HttpGet]
        public IEnumerable<Device> GetDevices()
        {
            return _devices;
        }

        [HttpGet("{id}")]
        public Device GetDeviceById([FromRoute] Guid id)
        {
            return _devices.FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// The device.
        /// </summary>
        public class Device
        {
            public Guid Id { get; set; }

            /// <summary>
            /// The ip address.
            /// </summary>
            public string IpAddress { get; set; }
        
            /// <summary>
            /// The name.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The access status.
            /// </summary>
            public bool Status => new Random().Next(100) >= 50;
        }
    }
}
