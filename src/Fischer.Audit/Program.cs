using Fischer.Audit.Persistence.Contexts;
using Fischer.Audit.Persistence.Repositories;
using Fischer.Audit.Persistence.Repositories.Interfaces;
using Fischer.Audit.Services;
using Fischer.Core.Application.StartupConfig;
using Fischer.Core.Infraestructure.Audit;

public sealed class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //!Important
        builder.Services.SetupFullEnvironment<Program>(builder);

        builder.Services.AddDbContext<AuditDbContext>();
        builder.Services.AddScoped<IAuditRepository, AuditRepository>();
        builder.Services.AddScoped<IAuditService, AuditService>();
        builder.Services.AddHostedService<NewAuditsQueueConsumer>();

        var app = builder.Build();

        //!Important
        app.StartupPreparation<AuditDbContext>(builder.Configuration);

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}