using Fischer.Core.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddLoggedUserConfig
{
    public static IServiceCollection AddAspNetUser(this IServiceCollection services)
    {
        services.AddScoped<IAspNetUser, AspNetUser>();
        return services;
    }
}