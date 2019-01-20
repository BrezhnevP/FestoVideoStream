using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FestoVideoStream.Attributes;

namespace FestoVideoStream.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class Device
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The ip address.
        /// </summary>
        [Required, IpAddress]
        public string IpAddress { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The configuration string of the device
        /// </summary>  
        public string Config { get; set; }

        /// <summary>
        /// The current status of the device
        /// </summary>
        public bool Status => new Random().Next(100) >= 50;
    }
}
