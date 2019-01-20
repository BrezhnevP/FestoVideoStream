using System;

namespace FestoVideoStream.Dto
{
    public class DeviceDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public string Config { get; set; }

        public bool Status { get; set; }
    }
}