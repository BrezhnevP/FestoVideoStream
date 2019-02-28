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
        private readonly IMapper _mapper;

        public DevicesService(IConfiguration configuration, IMapper mapper, DevicesContext context)
        {
            _configuration = configuration;
            _mapper = mapper;
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

        public async Task<DeviceDetailsDto> GetDevice(int id)
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

        public async Task<DeviceDetailsDto> CreateDevice(DeviceDetailsDto deviceDto)
        {
            var insertedDevice = _context.Devices.Add(_mapper.Map<Device>(deviceDto));
            await _context.SaveChangesAsync();

            return _mapper.Map<DeviceDetailsDto>(insertedDevice);
        }

        public async Task<bool> UpdateDevice(int id, DeviceDetailsDto deviceDto)
        {
            var device = _mapper.Map<Device>(deviceDto);

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

        public async Task<bool> DeleteDevice(int id)
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

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.Id == id);
        }

        private string GetDefaultConfig(int id)
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