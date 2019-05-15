using FestoVideoStream.Data;
using FestoVideoStream.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class DevicesService
    {
        private readonly AppDbContext _context;
        private readonly PathService pathService;

        public DevicesService(AppDbContext context, PathService pathService)
        {
            this.pathService = pathService;
            this._context = context;
        }

        public DbSet<Device> Devices => _context.Devices;

        public async Task<IQueryable<Device>> GetDevices(bool withStatus = true)
        {
            var devices = _context.Devices;
            
            if (withStatus)
            {
                var devicesAsync = await devices.ToListAsync();
                var devicesWithStatus = devicesAsync.Select(async device => new Device
                {
                    Id = device.Id,
                    Name = device.Name,
                    IpAddress = device.IpAddress,
                    Config = device.Config,
                    DeviceStatus = await this.GetDeviceStatus(device),
                    LastActivityDate = device.LastActivityDate,
                    StreamStatus = device.StreamStatus,
                    LastStreamStartDate = device.LastStreamStartDate,
                    LastStreamEndDate= device.LastStreamEndDate
                });

                return Task.WhenAll(devicesWithStatus).Result.AsQueryable();
            }

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await this._context.Devices.SingleOrDefaultAsync(d => d.Id == id);

            return device;
        }

        public async Task<Device> CreateDevice(Device device)
        {
            var insertedDevice = await this._context.Devices.AddAsync(device);
            await this._context.SaveChangesAsync();

            return insertedDevice.Entity;
        }

        public async Task<Device> UpdateDevice(Guid id, Device device)
        {
            this._context.Entry(device).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.DeviceExists(id).Result)
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

        public async Task<Device> UpdateDevice(Device device) => await this.UpdateDevice(device.Id, device);

        public async Task<bool> DeleteDevice(Guid id)
        {
            var device = await this._context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            this._context.Devices.Remove(device);
            await this._context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GetDeviceStatus(Device device) =>
            await ConnectionService.DeviceAvailable(device.IpAddress);

        public async Task<bool> DeviceExists(Guid id)
        {
            return await this._context.Devices.AnyAsync(e => e.Id == id);
        }

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1280x1024 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{this.pathService.RtmpUrl}/{id}\"";
    }
}