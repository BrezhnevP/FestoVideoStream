using System;

namespace FestoVideoStream.Dto
{
    public class DeviceDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public bool DeviceStatus { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public bool StreamingStatus { get; set; }

        public DateTime? LastStreamingDate { get; set; }
    }
}