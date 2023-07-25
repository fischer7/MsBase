//https://www.c-sharpcorner.com/blogs/api-gateway-in-net-60-using-ocelot
//https://github.com/Burgyn/MMLib.SwaggerForOcelot
/// Contribution: https://github.com/Burgyn/MMLib.SwaggerForOcelot/issues/261

using Ocelot.DependencyInjection;
using Ocelot.Middleware;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
        
        builder.Services.AddOcelot();
        builder.Services.AddSwaggerForOcelot(builder.Configuration);
                
        var app = builder.Build();

        app.UseCors();

        app.UsePathBase("/gate");

        
        app.UseSwaggerForOcelotUI(options =>
        {
            options.DownstreamSwaggerEndPointBasePath = "/gate/swagger/docs";
            options.PathToSwaggerGenerator = "/swagger/docs";
        }, uiOpt =>
        {
            uiOpt.DocumentTitle = "Gateway documentation";
        });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.UseOcelot().Wait();

        app.Run();
    }
}