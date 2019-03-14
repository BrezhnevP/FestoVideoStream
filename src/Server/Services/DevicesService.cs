using FestoVideoStream.Data;
using FestoVideoStream.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace FestoVideoStream.Services
{
    public class DevicesService
    {
        private readonly AppDbContext context;
        private readonly PathService _pathService;

        public DevicesService(AppDbContext context, PathService pathService)
        {
            this._pathService = pathService;
            this.context = context;
        }

        public async Task<IQueryable<Device>> GetDevices()
        {
            var devices = context.Devices;
            await devices.ForEachAsync(async device => device.Status = await GetDeviceStatus(device.Id));

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await context.Devices.SingleOrDefaultAsync(d => d.Id == id);
            device.Status = await GetDeviceStatus(device.Id);
            
            return device;
        }

        public async Task<Device> CreateDevice(Device device)
        {
            var insertedDevice = await context.Devices.AddAsync(device);
            await context.SaveChangesAsync();

            return insertedDevice.Entity;
        }

        public async Task<Device> UpdateDevice(Guid id, Device device)
        {
            context.Entry(device).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id).Result)
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
            var device = await context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            context.Devices.Remove(device);
            await context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> DeviceExists(Guid id)
        {
            return await context.Devices.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> GetDeviceStatus(Guid id) =>
            await _pathService.UrlExists(_pathService.GetDeviceDashManifest(id));

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1920x1200 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{_pathService.RtmpPath}/{id}\"";
    }
}