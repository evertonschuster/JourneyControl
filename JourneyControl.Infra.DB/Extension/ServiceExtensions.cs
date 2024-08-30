using JourneyControl.Application.Repositories;
using JourneyControl.Infra.DB.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyControl.Infra.DB.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfraDbServices(this IServiceCollection services)
        {
            services.AddSingleton<JourneyControlContext>();
            services.AddSingleton<IActivityRepository, ActivityRepository>();

            return services;
        }
    }
}
