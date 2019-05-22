using System;
using FestoVideoStream.Models.Enums;

namespace FestoVideoStream.Models.Dto
{
    public class DeviceDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IpAddress { get; set; }

        public int Port { get; set; }

        public string Config { get; set; }

        public bool DeviceStatus { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public bool StreamStatus { get; set; }

        public DateTime? LastStreamStartDate { get; set; }

        public DateTime? LastStreamEndDate { get; set; }

        public ConnectionCheckType CheckType { get; set; }
    }
}