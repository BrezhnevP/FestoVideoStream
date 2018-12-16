using System;
using System.Linq;
using System.Net;

namespace FestoVideoStream.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    /// <summary>
    /// The devices data controller.
    /// </summary>
    [Route("api/[controller]")]
    public class DevicesDataController : Controller
    {
        private readonly IEnumerable<Device> _devices = Enumerable.Range(1, new Random().Next(3, 10)).Select(x => new Device
        {
            Id = Guid.NewGuid(),
            IpAddress = IPAddress.Any.ToString(),
            Name = $"Device ¹{x}"
        });

        [HttpGet("[action]")]
        public IEnumerable<Device> GetDevices()
        {
            return _devices;
        }

        [HttpGet("[action]")]
        public Device GetDeviceById(Guid id)
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
