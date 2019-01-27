using AutoMapper;
using FestoVideoStream.Dto;
using FestoVideoStream.Entities;

namespace FestoVideoStream
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Device, DeviceDto>();
            CreateMap<Device, DeviceDetailsDto>();

            CreateMap<DeviceDetailsDto, DeviceDto>();
            CreateMap<DeviceDto, DeviceDetailsDto>();
        }
    }
}