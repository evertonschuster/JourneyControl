using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;

namespace JourneyControl.GUI.Extension
{
    internal static class ServiceExtensions
    {
        const string sourceName = "JourneyControl.Application.GUI";
        const string logName = "JourneyControl";

        public static void ApplicationLogging(this IServiceCollection services)
        {
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, logName);
            }

            services
            .AddLogging(logger =>
            {
                logger.AddEventLog(log =>
                {
                    log.SourceName = sourceName;
                    log.LogName = logName;
                });
            });
        }

        public static void ApplicationConfiguration(this IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}
