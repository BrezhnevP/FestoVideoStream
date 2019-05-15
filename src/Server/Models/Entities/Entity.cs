using System;
using System.ComponentModel.DataAnnotations;

namespace FestoVideoStream.Models.Entities
{
    public class Entity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// When entity is created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Last moment when entity is updated
        /// </summary>
        public DateTime Modified { get; set; }

    }
}