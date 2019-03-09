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
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The ip address.
        /// </summary>
        [Required, IpAddress]
        public string IpAddress { get; set; }

        /// <summary>
        /// The ip address.
        /// </summary>
        public IPAddress IPAddress => IPAddress.Parse(IpAddress);

        /// <summary>
        /// The name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The configuration string of the device
        /// </summary>  
        public string Config { get; set; }

        //TODO
        /// <summary>
        /// The current status of the device
        /// </summary>
        public bool Status => true; //new Ping().Send(this.ipAddress)?.Status == IPStatus.Success;


    }
}
