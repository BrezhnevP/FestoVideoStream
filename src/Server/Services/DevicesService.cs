using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FestoVideoStream.Data;
using FestoVideoStream.Dto;
using FestoVideoStream.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FestoVideoStream.Services
{
    public class DevicesService
    {
        private readonly DevicesContext _context;

        public DevicesService(DevicesContext context)
        {
            _context = context;
        }

        public IQueryable<DeviceDto> GetDevices()
        {
            var devices = _context.Devices.Select(device =>
                new DeviceDto
                {
                    Id = device.Id,
                    Name = device.Name,
                    IpAddress = device.IpAddress.ToString(),
                    Status = device.Status
                });

            return devices;
        }

        public async Task<DeviceDetailsDto> GetDevice(Guid id)
        {
            var device = await _context.Devices.Select(d => new DeviceDetailsDto
            {
                Id = d.Id,
                Name = d.Name,
                IpAddress = d.IpAddress.ToString(),
                Status = d.Status,
                Config = d.Config
            }).SingleOrDefaultAsync(d => d.Id == id);

            return device;
        }

        public async Task<Guid> CreateDevice(DeviceDetailsDto deviceDto)
        {
            var device = new Device
            {
                Id = Guid.NewGuid(),
                Name = deviceDto.Name,
                IpAddress = deviceDto.IpAddress,
                Config = deviceDto.Config
            };

            if (device.Config == null)
            {
                device.Config = GetDefaultConfig(device.Id);
            }

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return device.Id;
        }

        public async Task<bool> ModifyDevice(Guid id, DeviceDetailsDto device)
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
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
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

        private static string GetDefaultConfig(Guid id)
        {
            return
                "ffmpeg -f x11grab -s 1920x1200 " +
                "-framerate 15 -i :0.0 -c:v libx264 " +
                "-preset fast -pix_fmt yuv420p -s 1280x800 " +
                "-threads 0 -f flv " +
                $"\"rtmp://localhost/streamingapp/{id}\"";
        }
    }
}