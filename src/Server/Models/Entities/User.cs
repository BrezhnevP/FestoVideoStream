using System;
using System.ComponentModel.DataAnnotations;

namespace FestoVideoStream.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        [Key]
        public string Login { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string PasswordHash { get; set; }
    }
}
