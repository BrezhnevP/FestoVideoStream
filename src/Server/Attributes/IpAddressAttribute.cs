using FestoVideoStream.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            return 
                device.IpAddress == null || !Regex.IsMatch(device.IpAddress, "^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
                ? new ValidationResult("Incorrect IP address") 
                : ValidationResult.Success;
        }
    }
}