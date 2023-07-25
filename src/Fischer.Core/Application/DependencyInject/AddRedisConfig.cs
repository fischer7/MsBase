using Fischer.Core.Configurations.API;
using Fischer.Core.Configurations.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddRedisConfig
{
    public static IServiceCollection AddRedis(this IServiceCollection services, ConfigurationManager config, ApiConfigurations fullConfig)
    {
        if (fullConfig.UseRedis)
        {
            var redisConfigs = config.GetSection("Redis").Get<RedisConfig>();

            services.AddStackExchangeRedisCache(redisCache =>
            {
                redisCache.Configuration = redisConfigs?.Endpoint;
                redisCache.InstanceName = redisConfigs?.InstanceName;
            });

            services.AddSession(options =>
            {
                options.Cookie.Name = redisConfigs?.SessionName;
                if (redisConfigs?.TTL is not null)
                    options.IdleTimeout = TimeSpan.FromHours(redisConfigs!.TTL);
            });
        }
        return services;
    }
}