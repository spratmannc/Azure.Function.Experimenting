using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WorkerService1.Services;

namespace WorkerService1
{
    public class HeartbeatService : BackgroundService
    {
        private readonly ILogger<HeartbeatService> log;
        private readonly RemoteService remote;

        public HeartbeatService(ILogger<HeartbeatService> log, RemoteService remote)
        {
            this.log = log;
            this.remote = remote;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);

            log.LogInformation("Monitoring has begun...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await remote.SendHeartbeat();

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
