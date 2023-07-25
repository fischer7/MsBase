using Fischer.Auth.Services;

namespace Fischer.Auth.Configurations;
internal static class DomainServicesConfiguration
{
    internal static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthCacheService, AuthCacheService>();
        return services;
    }
}
