using Fischer.Auth.Persistence.Context;
using Fischer.Auth.Persistence.Repositories;
using Fischer.Auth.Persistence.Repositories.Interfaces;

namespace Fischer.Auth.Configurations;
internal static class DatabaseConfiguration
{
    public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services)
    {
        RegisterSeed(services);
        RegisterRepositories(services);
        return services;
    }

    private static void RegisterSeed(IServiceCollection services)
    {
        services.AddScoped<SeedAuthDbContext>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
