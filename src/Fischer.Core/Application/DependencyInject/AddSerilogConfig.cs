using Destructurama;
using Fischer.Core.Application.DependencyInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddSerilogConfig
{
    public static IServiceCollection AddSerilog(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddLogging(logging => logging.AddSerilog());

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .Destructure.UsingAttributes()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        return services;
    }
}