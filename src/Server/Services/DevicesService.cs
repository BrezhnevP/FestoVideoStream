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
        private readonly ConnectionService connectionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DevicesService"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="pathService">
        /// The path service.
        /// </param>
        /// <param name="connectionService">
        /// The connection service.
        /// </param>
        public DevicesService(AppDbContext context, PathService pathService, ConnectionService connectionService)
        {
            this.pathService = pathService;
            this.connectionService = connectionService;
            this.context = context;
        }

        public async Task<IQueryable<Device>> GetDevices()
        {
            var devices = this.context.Devices;
            await devices.ForEachAsync(async device => device.Status = await GetDeviceStatus(device.Id));

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await this.context.Devices.SingleOrDefaultAsync(d => d.Id == id);
            device.Status = await this.GetDeviceStatus(device.Id);
            
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
            var device = await this.context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            this.context.Devices.Remove(device);
            await this.context.SaveChangesAsync();

            return true;
        }

        private async Task<bool> DeviceExists(Guid id)
        {
            return await this.context.Devices.AnyAsync(e => e.Id == id);
        }

        private async Task<bool> GetDeviceStatus(Guid id) =>
            await this.connectionService.UrlExists(this.pathService.GetDeviceDashManifest(id));

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1920x1200 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{this.pathService.RtmpUrl}/{id}\"";
    }
}