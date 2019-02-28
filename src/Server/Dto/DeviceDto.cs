using System;

namespace FestoVideoStream.Dto
{
    public class DeviceDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public bool Status { get; set; }
    }
}