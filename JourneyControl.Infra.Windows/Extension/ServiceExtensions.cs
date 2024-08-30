using JourneyControl.Application.Services;
using JourneyControl.Infra.Windows.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyControl.Infra.Windows.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddWindowsApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IActivityMonitor, ActivityMonitor>();

            return services;
        }
    }
}
