using Fischer.Core.Configurations.API;
using Fischer.Core.Services.Statistics;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddStatisticsConfig
{
    public static IServiceCollection AddStatistics(this IServiceCollection services, ApiConfigurations config)
    {
        if (config.EnableMediatrPerformancePipeline)
            services.AddScoped<IStatisticsService, StatisticsService>();

        return services;
    }
}