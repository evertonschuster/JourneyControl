using JourneyControl.Application.Services;
using JourneyControl.Application.Services.SqlScriptExecutor;
using JourneyControl.Infra.Windows.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyControl.Infra.Windows.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddWindowsApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IActivityMonitor, ActivityMonitor>();

            services.AddSingleton<IDirectoryFinder, ScriptDirectoryFinder>();

            return services;
        }
    }
}
