using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FestoVideoStream.Models.Entities;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        private static readonly Regex IpRegex =
            new Regex(
                @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            return
                device.IpAddress == null || !IpRegex.IsMatch(device.IpAddress)
                ? new ValidationResult("Incorrect IP address") 
                : ValidationResult.Success;
        }
    }
}