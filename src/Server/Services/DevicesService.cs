using FestoVideoStream.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FestoVideoStream.Models.Entities;
using FestoVideoStream.Models.Enums;

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

        private DbSet<Device> Devices => _context.Devices;

        public async Task<IQueryable<Device>> GetDevices(bool withStatus = true)
        {
            if (withStatus)
            {
                var devicesAsync = await Devices.ToListAsync();
                var devicesWithStatus = devicesAsync.Select(async device => new Device
                {
                    Id = device.Id,
                    Name = device.Name,
                    IpAddress = device.IpAddress,
                    Config = device.Config,
                    DeviceStatus = await this.CheckDeviceStatus(device),
                    LastActivityDate = device.LastActivityDate,
                    StreamStatus = device.StreamStatus,
                    LastStreamStartDate = device.LastStreamStartDate,
                    LastStreamEndDate= device.LastStreamEndDate
                });

                return Task.WhenAll(devicesWithStatus).Result.AsQueryable();
            }

            return Devices;
        }

        public async Task<Device> GetDevice(Guid id)
        {
            var device = await Devices.SingleOrDefaultAsync(d => d.Id == id);

            return device;
        }

        public async Task<Device> CreateDevice(Device device)
        {
            var insertedDevice = await Devices.AddAsync(device);
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

        public async Task UpdateDevices(IEnumerable<Device> devices)
        {
            foreach (var device in devices)
            {
                this._context.Entry(device).State = EntityState.Modified;
            }

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return;
            }
        }

        public async Task<bool> DeleteDevice(Guid id)
        {
            var device = await Devices.FindAsync(id);
            if (device == null)
            {
                return false;
            }

            Devices.Remove(device);
            await this._context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckDeviceStatus(Device device)
        {
            if (device.CheckType == ConnectionCheckType.Tcp)
                return await ConnectionService.CheckConnectionByTcp(device.IpEndPoint);
            else
                return await ConnectionService.CheckConnectionByPing(device.IpEndPoint);
        }
            

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