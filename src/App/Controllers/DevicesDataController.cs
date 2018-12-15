using System;

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
        /// <summary>
        /// The devices.
        /// </summary>
        private static readonly List<Device> Devices = new List<Device>
                                                           {
                                                               new Device
                                                                   {
                                                                       IpAddress = "255.255.255.255",
                                                                       Name = "Device 1"
                                                                   },
                                                               new Device
                                                                   {
                                                                       IpAddress = "255.255.255.255",
                                                                       Name = "Device 2"
                                                                   },
                                                               new Device
                                                                   {
                                                                       IpAddress = "255.255.255.255",
                                                                       Name = "Device 3"
                                                                   }
                                                           };

        /// <summary>
        /// The get devices.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        [HttpGet("[action]")]
        public IEnumerable<Device> GetDevices()
        {
            return Devices;
        }

        /// <summary>
        /// The device.
        /// </summary>
        public class Device
        {
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
