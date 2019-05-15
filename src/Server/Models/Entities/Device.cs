using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FestoVideoStream.Attributes;

namespace FestoVideoStream.Models.Entities
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
        /// The device status.
        /// </summary>
        [NotMapped]
        public bool DeviceStatus { get; set; }

        public DateTime? LastActivityDate { get; set; }

        /// <summary>
        /// The device's stream status.
        /// </summary>
        public bool StreamStatus { get; set; }

        public DateTime? LastStreamStartDate { get; set; }

        public DateTime? LastStreamEndDate { get; set; }
    }
}
