using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddCstCors
{
    internal static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(setupAction =>
        {
            setupAction.AddPolicy(name: "CorsPolicy", configurePolicy =>
            {
                configurePolicy.WithOrigins("https://localhost:*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }


};