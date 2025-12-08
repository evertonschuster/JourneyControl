using JourneyControl.Application.Services;
using JourneyControl.Application.Services.SqlScriptExecutor;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyControl.Application.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IActivityService, ActivityService>();
            services.AddSingleton<ScriptExecutionAppService>();
            services.AddSingleton<ScriptExecutionOptions>();

            return services;
        }
    }
}
