using App.WindowsService;
using JourneyControl.Application.Extension;
using JourneyControl.Infra.DB.Extension;
using JourneyControl.Infra.Windows.Extension;
using System.Diagnostics;

const string sourceName = "JourneyControl.Application.Services";
const string logName = "JourneyControl";

if (!EventLog.SourceExists(sourceName))
{
    EventLog.CreateEventSource(sourceName, logName);
}

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddWindowsService(options =>
    {
        options.ServiceName = sourceName;
    })
    .AddLogging(e =>
    {
        e.AddEventLog(log =>
        {
            log.SourceName = sourceName;
            log.LogName = logName;
        });
    });

builder.Services.AddHostedService<Worker>();
builder.Services.AddApplicationServices();
builder.Services.AddWindowsApplicationServices();
builder.Services.AddInfraDbServices();

var host = builder.Build();
host.Run();
