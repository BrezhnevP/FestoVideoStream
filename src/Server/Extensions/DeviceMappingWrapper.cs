using AutoMapper;
using FestoVideoStream.Models.Dto;
using FestoVideoStream.Models.Entities;

namespace FestoVideoStream.Extensions
{
    public static class DeviceMappingWrapper
    {
        public static Device ToEntity(this DeviceDto deviceDto) => Mapper.Map<Device>(deviceDto);

        public static Device ToEntity(this DeviceDetailsDto deviceDetailsDto) => Mapper.Map<Device>(deviceDetailsDto);
        
        public static DeviceDto ToDto(this Device device) => Mapper.Map<DeviceDto>(device);
        
        public static DeviceDto ToDto(this DeviceDetailsDto deviceDetailsDto) => Mapper.Map<DeviceDto>(deviceDetailsDto);
        
        public static DeviceDetailsDto ToDetailsDto(this Device device) => Mapper.Map<DeviceDetailsDto>(device);

        public static DeviceDetailsDto ToDetailsDto(this DeviceDto deviceDto) =>
            Mapper.Map<DeviceDetailsDto>(deviceDto);
    }
}