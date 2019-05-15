using System;

namespace FestoVideoStream.Models.Dto
{
    public class DeviceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public bool DeviceStatus { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public bool StreamStatus { get; set; }

        public DateTime? LastStreamStartDate { get; set; }

        public DateTime? LastStreamEndDate { get; set; }
    }
}