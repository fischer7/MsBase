using Fischer.Core.Configurations.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fischer.Core.Application.DependencyInject;
public static class AddSwaggerConfig
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, SwaggerConfig? swaggerConfig)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SchemaFilter<EnumSchemaFilter>();
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = $"Fischer Enterprise - API {swaggerConfig?.ApiName ?? "with Swagger"}",
                Version = "1.0",
                Description = "This API is part of...",
                Contact = new OpenApiContact() { Name = "Felipe Fischer", Email = "felipe@fischer.dev.br" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/Licenses/MIT") }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Add you JWT token: Bearer {token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });


            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

        });
        return services;
    }


    public static IApplicationBuilder UseSwaggerConfiguration(
        this IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IConfiguration configuration)
    {
        var swaggerConfig = configuration.GetSection("SwaggerConfig").Get<SwaggerConfig>();

        app.UseSwagger();
        if (swaggerConfig is not null)
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", swaggerConfig.ApiName);
                    }
                });
        return app;
    }

    //https://stackoverflow.com/a/60091019/11396875
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                model.Enum.Clear();
                Enum.GetNames(context.Type)
                    .ToList()
                    .ForEach(n => model.Enum.Add(new OpenApiString(n)));
            }
        }
    }
}
