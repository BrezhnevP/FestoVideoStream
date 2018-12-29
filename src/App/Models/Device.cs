using System;

namespace FestoVideoStream.Models
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
        public string IpAddress { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public bool Status { get; set; }
        
    }
}
