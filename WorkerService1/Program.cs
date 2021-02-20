using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using System;
using WorkerService1.Models;
using WorkerService1.Services;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                    var settings = new Settings();
                    hostContext.Configuration.Bind(settings);

                    services.AddRefitClient<IFunctionService>()
                            .ConfigureHttpClient(http =>
                            {
                                http.BaseAddress = new Uri(settings.RemoteServiceUrl);
                                http.DefaultRequestHeaders.Add("x-functions-key", settings.ApiKey);
                            });

                    services.AddTransient<RemoteService>();

                    services.AddHostedService<HeartbeatService>();
                    services.AddHostedService<EventLogWatcherService>();
                    services.AddHostedService<EventLogWriterService>();

                });
    }
}
