using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class TimedDeviceStatusService : IHostedService, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private Timer timer;

        public TimedDeviceStatusService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(this.CheckDevicesStreamStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        private async void CheckDevicesStreamStatus(object state)
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var devicesService =
                    scope.ServiceProvider
                        .GetRequiredService<DevicesService>();

                var devices = await devicesService.GetDevices(false);
                await devices.ForEachAsync(async device =>
                {
                    var isActive = await devicesService.GetDeviceStreamStatus(device);
                    var isStreaming = await devicesService.GetDeviceStreamStatus(device);
                    if (isActive || isStreaming)
                    {
                        if (isActive)
                        {
                            device.LastActivityDate = DateTime.UtcNow;
                        }
                        if (isStreaming)
                        {
                            device.LastStreamingDate = DateTime.UtcNow;
                        }
                        await devicesService.UpdateDevice(device);
                    }
                });
            }
        }
    }
}