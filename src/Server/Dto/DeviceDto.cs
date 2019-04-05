using System;

namespace FestoVideoStream.Dto
{
    public class DeviceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public bool DeviceStatus;

        public DateTime? LastActivityDate { get; set; }

        public bool StreamingStatus;

        public DateTime? LastStreamingDate { get; set; }
    }
}