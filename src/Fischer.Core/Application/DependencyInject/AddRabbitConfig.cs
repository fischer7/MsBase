//https://github.com/thinktecture-labs/webinar-keycloak-authorization/tree/main/src/api/Authorization/RPT

using Fischer.Core.Configurations.API;
using Fischer.Core.Infraestructure.Queues;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
public static class AddRabbitConfig
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, ApiConfigurations config)
    {
        if (config.UseRabbitMq)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddSingleton<IEmailBus, EmailBus>();
        }
        return services;
    }
}