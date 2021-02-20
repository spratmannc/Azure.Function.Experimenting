/*  
    HINT:  Warnings for using EventLogQuery and EventLogWatcher don't appear
           because we are specifically targeting windows:  
           https://docs.microsoft.com/en-us/dotnet/standard/frameworks

    HINT:  Use the Event Viewer utility in Windows to build the filters graphically,
           then copy the Path and Query to use to build the xpath.
           Filter examples:  https://docs.microsoft.com/en-us/previous-versions/bb399427(v=vs.90)
*/

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkerService1.Services;

namespace WorkerService1
{
    public class EventLogWatcherService : BackgroundService
    {
        private readonly ILogger<EventLogWatcherService> log;
        private readonly RemoteService remote;
        EventLogWatcher watcher;

        public EventLogWatcherService(ILogger<EventLogWatcherService> log, RemoteService remote)
        {
            this.log = log;
            this.remote = remote;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            log.LogInformation("Retrieving event log queries");

            var queries = await remote.GetEventLogFilters();

            log.LogInformation("Received {count} queries", queries.Count());

            ConfigureWatcher(queries);
        }

        private void ConfigureWatcher(IEnumerable<string> queries)
        {
            // TODO: incorporate queries into xpath
            var xpath = "*[System[Provider[@Name='MyTesting'] and (Level=3)]]";

            var query = new EventLogQuery("Application", PathType.LogName, xpath);

            watcher = new EventLogWatcher(query);

            watcher.EventRecordWritten += OnEventDetectedAsync;
            watcher.Enabled = true;
        }

        private async void OnEventDetectedAsync(object sender, EventRecordWrittenEventArgs e)
        {
            var message = e.EventRecord.FormatDescription();

            log.LogInformation("SENDING: {message}", e.EventRecord.FormatDescription());

            await remote.Warn(message);
        }

        public override void Dispose()
        {
            watcher.EventRecordWritten -= OnEventDetectedAsync;
            base.Dispose();
        }
    }
}
