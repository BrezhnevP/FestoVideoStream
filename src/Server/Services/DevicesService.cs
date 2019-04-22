using FestoVideoStream.Data;
using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class DevicesService
    {
        private readonly AppDbContext context;
        private readonly PathService pathService;

        public DevicesService(AppDbContext context, PathService pathService)
        {
            this.pathService = pathService;
            this.context = context;
        }

        public DbSet<Device> Devices => context.Devices;

        public async Task<IQueryable<Device>> GetDevices(bool withStatus = true)
        {
            var devices = context.Devices;
            if (withStatus)
            {
                return devices.Select(device => new Device
                {
                    Id = device.Id,
                    Name = device.Name,
                    IpAddress = device.IpAddress,
                    Config = device.Config,
                    DeviceStatus = this.GetDeviceStreamStatus(device).Result,
                    LastActivityDate = device.LastActivityDate,
                    LastStreamingDate = device.LastStreamingDate
                });
            }

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await this.context.Devices.SingleOrDefaultAsync(d => d.Id == id);
            device.DeviceStatus = await this.GetDeviceStreamStatus(device.Id);
            
            return device;
        }

        public async Task<Device> CreateDevice(Device device)
        {
            var insertedDevice = await this.context.Devices.AddAsync(device);
            await this.context.SaveChangesAsync();

            return insertedDevice.Entity;
        }

        public async Task<Device> UpdateDevice(Guid id, Device device)
        {
            this.context.Entry(device).State = EntityState.Modified;

            try
            {
                await this.context.SaveChangesAsync();
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
            var device = await this.context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            this.context.Devices.Remove(device);
            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> GetDeviceStreamStatus(Guid id) =>
            await ConnectionService.UrlExistsAsync(this.pathService.GetDeviceDashManifest(id));

        public async Task<bool> GetDeviceStreamStatus(Device device) =>
            await ConnectionService.UrlExistsAsync(this.pathService.GetDeviceDashManifest(device.Id));

        public async Task SetDeviceStreamStatus(Device device)
        {
            device.DeviceStatus = await GetDeviceStreamStatus(device.Id);
        }

        public async Task<bool> DeviceExists(Guid id)
        {
            return await this.context.Devices.AnyAsync(e => e.Id == id);
        }

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1280x1024 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{this.pathService.RtmpUrl}/{id}\"";
    }
}