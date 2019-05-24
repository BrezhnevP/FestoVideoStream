using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FestoVideoStream.Models.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Key]
        public string Login { get; set; }

        /// <summary>
        ///     This property is used to map with UserDto.Password
        /// </summary>
        [NotMapped]
        public string Password { get; set; }

        public string PasswordHash { get; set; }
    }
}
