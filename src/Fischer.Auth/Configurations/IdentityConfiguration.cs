using Fischer.Auth.Domain.Entities;
using Fischer.Auth.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace Fischer.Auth.Configurations;
internal static class IdentityConfiguration
{
    internal static IServiceCollection AddIdentityConfigurations(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
            options.User.AllowedUserNameCharacters = string.Empty;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AuthDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
