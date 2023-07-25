using Fischer.Email.Services;
using Fischer.Core.Application.StartupConfig;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        //!Important
        builder.Services.SetupFullEnvironment<Program>(builder);

        builder.Services.AddHostedService<EmailQueueConsumer>();

        var app = builder.Build();
        
        //!Important
        app.StartupPreparation(builder.Configuration);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}