using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FestoVideoStream.Attributes;
using FestoVideoStream.Services;

namespace FestoVideoStream.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        [Required, IpAddress]
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public bool Status;
    }
}
