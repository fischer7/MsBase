using Fischer.Core.Application.DependencyInject;
using Fischer.Core.Configurations;
using Fischer.Core.Configurations.API;
using Fischer.Core.Configurations.Swagger;
using Fischer.Core.Persistence.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.StartupConfig;
public static class StartupConfig
{
    public static IServiceCollection SetupFullEnvironment<TProgram>(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        var type = typeof(TProgram);
        if (builder.Configuration == null)
            throw new ArgumentException(nameof(builder.Configuration));

        var configuration = builder.Configuration;
        var swaggerConfig = configuration.GetSection("SwaggerConfig").Get<SwaggerConfig>();

        builder.Services.Configure<ApiConfigurations>(
            builder.Configuration.GetSection(typeof(ApiConfigurations).Name));

        var appSettingsAccess = new AppsettingsGet(builder.Environment, configuration);
        services.AddSingleton(appSettingsAccess);

        var fullConfig = configuration.GetSection(typeof(ApiConfigurations).Name).Get<ApiConfigurations>();
        fullConfig!.AssemblyType = type;

        services
            .AddHttpContextAccessor()
            .AddAspNetUser()
            .AddNotifications(fullConfig)
            .AddSingleton(fullConfig)
            .AddApiVersionConfigurations()
            .AddRabbitMq(fullConfig)
            .AddSwaggerConfiguration(swaggerConfig)
            .AddSerilog(configuration)
            .AddRedis(configuration, fullConfig)
            .AddStringEnumConfig()
            .AddCustomCors()
            .AddStatistics(fullConfig)
            .AddMediatorConfiguration(fullConfig)
            .AddAuditConfiguration()
            .AddJwt(fullConfig, configuration)
            .AddHealthChecks();

        return services;
    }

    public static WebApplication? StartupPreparation<TDbContext>(this WebApplication? app, 
        ConfigurationManager configuration) where TDbContext : CoreDbContext
    {
        if(app is null)
            throw new ArgumentNullException(nameof(app));

        var type = typeof(TDbContext);

        using (var scope = app?.Services.CreateScope())
        {
            if (scope is null)
                return app;
            var appSettings = (ApiConfigurations)scope.ServiceProvider.GetRequiredService(typeof(ApiConfigurations));
            if (appSettings?.RunMigrations ?? false)
            {
                var dbContext = (CoreDbContext)scope.ServiceProvider.GetRequiredService(type);
                dbContext.Database.Migrate();
            }

            var provider = (IApiVersionDescriptionProvider)scope.ServiceProvider.GetRequiredService(typeof(IApiVersionDescriptionProvider));
            app!.UseSwaggerConfiguration(provider, configuration);
        }

        app!.UseCors("CorsPolicy");

        return app;
    }

    public static WebApplication? StartupPreparation(this WebApplication? app,
    ConfigurationManager configuration)
    {
        if (app is null)
            throw new ArgumentNullException(nameof(app));

        using (var scope = app?.Services.CreateScope())
        {
            if (scope is null)
                return app;

            var provider = (IApiVersionDescriptionProvider)scope.ServiceProvider.GetRequiredService(typeof(IApiVersionDescriptionProvider));
            app!.UseSwaggerConfiguration(provider, configuration);
        }

        app!.UseCors("CorsPolicy");

        return app;
    }

}
