using Fischer.Auth.Configurations;
using Fischer.Auth.Persistence.Context;
using Fischer.Core.Application.DependencyInject;
using Fischer.Core.Application.StartupConfig;
using Fischer.Core.Configurations.API;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.SetupFullEnvironment<Program>(builder);

        builder.Services.AddDbContext<AuthDbContext>();

        builder.Services
            .RegisterDatabaseServices()
            .RegisterDomainServices()
            .AddIdentityConfigurations();

        var app = builder.Build();
        if (app is null)
            throw new ArgumentNullException(nameof(app));

        using (var scope = app?.Services.CreateScope())
        {
            if (scope is not null)
            {
                var appSettings = (ApiConfigurations)scope.ServiceProvider.GetRequiredService(typeof(ApiConfigurations));
                var seedAuthDbContext = (SeedAuthDbContext)scope.ServiceProvider.GetRequiredService(typeof(SeedAuthDbContext));
                if (appSettings?.RunMigrations ?? false)
                {
                    var dbContext = (AuthDbContext)scope.ServiceProvider.GetRequiredService(typeof(AuthDbContext));
                    dbContext.Database.Migrate();
                }
                seedAuthDbContext.Seed().Wait();

                var provider = (IApiVersionDescriptionProvider)scope.ServiceProvider.GetRequiredService(typeof(IApiVersionDescriptionProvider));
                app!.UseSwaggerConfiguration(provider, builder.Configuration);
            }
        }
        if (app is null)
            throw new ArgumentNullException("Fail to load, app is null");

        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapHealthChecks("/health");

        app.Run();
    }
}