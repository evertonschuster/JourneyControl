using JourneyControl.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyControl.Application.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IActivityService, ActivityService>();

            return services;
        }
    }
}
