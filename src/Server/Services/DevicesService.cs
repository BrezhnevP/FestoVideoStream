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
        private readonly DevicesContext context;
        private readonly UrlService urlService;

        public DevicesService(DevicesContext context, UrlService urlService)
        {
            this.urlService = urlService;
            this.context = context;
        }

        public IQueryable<Device> GetDevices()
        {
            var devices = context.Devices.Select(d => new Device
            {
                Id = d.Id,
                Name = d.Name,
                IpAddress = d.IpAddress,
                Config = d.Config,
                Status = GetDeviceStatus(d.Id)
            });

            return devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await context.Devices.SingleOrDefaultAsync(d => d.Id == id);
            device.Status = GetDeviceStatus(device.Id);
            
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
            var device = await context.Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            context.Devices.Remove(device);
            await context.SaveChangesAsync();

            return true;
        }

        private bool DeviceExists(Guid id)
        {
            return context.Devices.Any(e => e.Id == id);
        }

        private bool GetDeviceStatus(Guid id) =>
            urlService.UrlExists(urlService.GetDeviceRtmpPath(id));

        private string GetDefaultConfig(Guid id) =>
            "ffmpeg -f x11grab -s 1920x1200 " +
            "-framerate 15 -i :0.0 -c:v libx264 " +
            "-preset fast -pix_fmt yuv420p -s 1024x800 " +
            "-threads 0 -f flv " +
            $"\"{urlService.RtmpPath}/{id}\"";
    }
}