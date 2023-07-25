//https://stackoverflow.com/questions/64145381/iservicecollection-does-not-contain-a-definition-for-addversionedapiexplorer

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddApiVersionConfig
{

    internal static IServiceCollection AddApiVersionConfigurations(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}