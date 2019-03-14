using System;
using System.ComponentModel.DataAnnotations;

namespace FestoVideoStream.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Key]
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
