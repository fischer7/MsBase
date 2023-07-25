//https://stackoverflow.com/a/58155532/11396875
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Fischer.Core.Application.DependencyInject;
internal static class StringEnumConfig
{
    internal static IServiceCollection AddStringEnumConfig(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(opt =>
        {
            opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            opt.SerializerSettings.Converters.Add(new StringEnumConverter());
        });

        return services;
    }
}