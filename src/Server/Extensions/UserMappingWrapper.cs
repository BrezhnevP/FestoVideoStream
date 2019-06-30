using AutoMapper;
using FestoVideoStream.Models.Dto;
using FestoVideoStream.Models.Entities;

namespace FestoVideoStream.Extensions
{
    public static class UserMappingWrapper
    {
        public static User ToEntity(this UserDto userDto) => Mapper.Map<User>(userDto);

        public static UserDto ToDto(this User user) => Mapper.Map<UserDto>(user);
    }
}