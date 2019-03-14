using System;

namespace FestoVideoStream.Dto
{
    public class UserDto
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Password { get; set; }
    }
}