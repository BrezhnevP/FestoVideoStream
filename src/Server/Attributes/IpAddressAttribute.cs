using FestoVideoStream.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace FestoVideoStream.Attributes
{
    public class IpAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var device = (Device) validationContext.ObjectInstance;
            try
            {
                IPAddress.Parse(device.IpAddress);
            }
            catch (FormatException e)
            {
                return new ValidationResult("Incorrect IP address");
            }

            return ValidationResult.Success;
        }
    }
}