using FestoVideoStream.Entities;
using System.ComponentModel.DataAnnotations;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            return device.IpAddress == null 
                ? new ValidationResult("Incorrect IP address") 
                : ValidationResult.Success;
        }
    }
}