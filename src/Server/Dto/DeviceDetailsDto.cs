using System;

namespace FestoVideoStream.Dto
{
    public class DeviceDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public string Config { get; set; }

        /// <summary>
        /// The device status.
        /// </summary>
        public bool DeviceStatus;

        /// <summary>
        /// The status.
        /// </summary>
        public DateTime? LastActivityDate { get; set; }

        /// <summary>
        /// The device's stream status.
        /// </summary>
        public bool StreamingStatus;

        /// <summary>
        /// The status.
        /// </summary>
        public DateTime? LastStreamingDate { get; set; }
    }
}