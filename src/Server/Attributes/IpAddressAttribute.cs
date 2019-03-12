using FestoVideoStream.Entities;
using System.ComponentModel.DataAnnotations;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            if (device.IpAddress == null)
                return new ValidationResult("Incorrect IP address");

            return ValidationResult.Success;
        }
    }
}