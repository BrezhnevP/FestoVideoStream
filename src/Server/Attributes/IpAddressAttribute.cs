using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using FestoVideoStream.Models.Entities;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            return
                device.IpAddress == null || IPAddress.TryParse(device.IpAddress, out var address)
                ? new ValidationResult("Incorrect IP address") 
                : ValidationResult.Success;
        }
    }
}