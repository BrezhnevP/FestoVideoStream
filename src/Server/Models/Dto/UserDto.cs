using System;

namespace FestoVideoStream.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}