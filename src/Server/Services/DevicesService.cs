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
        private readonly AppDbContext _context;
        private readonly PathService _pathService;

        public DevicesService(AppDbContext context, PathService pathService)
        {
            this._pathService = pathService;
            this._context = context;
        }

        public async Task<IQueryable<Device>> GetDevices()
        {
            var devices = this._context.Devices;
            await devices.ForEachAsync(async device => device.DeviceStatus = await GetDeviceStreamStatus(device.Id));

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await this._context.Devices.SingleOrDefaultAsync(d => d.Id == id);
            device.DeviceStatus = await this.GetDeviceStreamStatus(device.Id);
            
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

        public async Task<Device> UpdateDevice(Device device) => await UpdateDevice(device.Id, device);

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

        public async Task<bool> GetDeviceStreamStatus(Guid id) =>
            await ConnectionService.UrlExists(this._pathService.GetDeviceDashManifest(id));

        public async Task<bool> GetDeviceStreamStatus(Device device) =>
            await ConnectionService.UrlExists(this._pathService.GetDeviceDashManifest(device.Id));

        private async Task<bool> DeviceExists(Guid id)
        {
            return await this._context.Devices.AnyAsync(e => e.Id == id);
        }

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1920x1200 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{this._pathService.RtmpUrl}/{id}\"";
    }
}