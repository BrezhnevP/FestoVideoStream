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
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public TimedDeviceStatusService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckDevicesStreamStatus, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async void CheckDevicesStreamStatus(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var devicesService =
                    scope.ServiceProvider
                        .GetRequiredService<DevicesService>();

                var devices = await devicesService.GetDevices();
                await devices.ForEachAsync(async device =>
                {
                    var isStreaming = await devicesService.GetDeviceStreamStatus(device);
                    if (isStreaming)
                    {
                        device.LastStreamingDate = DateTime.Now;
                        await devicesService.UpdateDevice(device);
                    }
                });
            }
        }
    }
}