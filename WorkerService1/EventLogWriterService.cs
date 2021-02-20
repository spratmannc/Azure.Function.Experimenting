using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkerService1.Services;

namespace WorkerService1
{
    public class EventLogWriterService : BackgroundService
    {
        private readonly ILogger<EventLogWriterService> log;
        private readonly RemoteService remote;
        private readonly EventLog eventLog;

        public EventLogWriterService(ILogger<EventLogWriterService> log, RemoteService remote)
        {
            this.log = log;
            this.remote = remote;
            this.eventLog = new EventLog("Application");
            eventLog.Source = "MyTesting";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queries = await remote.GetEventLogFilters();

            log.LogInformation("Triggering errors");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10 * 1000);

                var message = queries.First();

                eventLog.WriteEntry(message, EventLogEntryType.Warning, 1001);
            }
        }
    }
}
