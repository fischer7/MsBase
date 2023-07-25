using Fischer.Core.Configurations.API;
using Fischer.Core.Services.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddApiNotifications
{
    internal static IServiceCollection AddNotifications(this IServiceCollection services, ApiConfigurations config)
    {
        if (config.UseApiNotifications)
            services.AddScoped<IApiNotification, ApiNotification>();

        return services;
    }
}