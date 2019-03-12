using AutoMapper;
using FestoVideoStream.Data;
using FestoVideoStream.Dto;
using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class DevicesService
    {
        private readonly DevicesContext _context;
        private readonly IConfiguration _configuration;

        public DevicesService(IConfiguration configuration, DevicesContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public IQueryable<Device> GetDevices()
        {
            var devices = _context.Devices;

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await _context.Devices.SingleOrDefaultAsync(d => d.Id == id);

            return device;
        }

        public async Task<Device> CreateDevice(Device device)
        {
            var insertedDevice = await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            return insertedDevice.Entity;
        }

        public async Task<Device> UpdateDevice(Guid id, Device device)
        {
            _context.Entry(device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return device;
        }

        public async Task<bool> DeleteDevice(Guid id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool DeviceExists(Guid id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }

        private string GetDefaultConfig(Guid id)
        {
            
            var configurationString = "ffmpeg -f x11grab -s 1920x1200 " +
                                      "-framerate 15 -i :0.0 -c:v libx264 " +
                                      "-preset fast -pix_fmt yuv420p -s 1024x800 " +
                                      "-threads 0 -f flv " +
                                      $"\"{_configuration.GetValue<string>("RtmpServerPath")}/dash/{id}\"";

            return configurationString;
        }
    }
}