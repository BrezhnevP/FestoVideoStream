using System;

namespace FestoVideoStream.Entities
{
    /// <summary>
    /// The device.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Password { get; set; }
    }
}
