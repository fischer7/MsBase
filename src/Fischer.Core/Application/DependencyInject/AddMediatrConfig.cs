using Fischer.Core.Application.Behaviours;
using Fischer.Core.Configurations.API;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddMediatrConfig
{
    internal static IServiceCollection AddMediatorConfiguration(this IServiceCollection services,
        ApiConfigurations fullConfig)
    {
        //MediatR.Extensions.Microsoft.DependencyInjection
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(fullConfig.AssemblyType!.Assembly);
        });

        services.AddScoped<IMediator, Mediator>();

        services.RegisterMediatorHandlers(fullConfig);

        if (fullConfig.EnableMediatrLogPipeline)
            services.LogPipelineBehavior();

        if (fullConfig.EnableMediatrExceptionPipeline)
            services.ExceptionPipelineBehavior();

        if (fullConfig.EnableMediatrFailFastPipeline)
            services.FailFastPipeline(fullConfig);

        if (fullConfig.EnableMediatrPerformancePipeline)
            services.PerformancePipelineBehavior();

        return services;
    }

    internal static void RegisterMediatorHandlers(this IServiceCollection services, ApiConfigurations fullConfig)
    {
        var assembly = fullConfig.AssemblyType!.Assembly;

        var classesDaApplication = assembly.ExportedTypes
            .Select(t => t.GetTypeInfo())
            .Where(t => t.IsClass && !t.IsAbstract);

        var nomeDasInterfacesHandlers = new List<string>
                {
                    typeof(IRequestHandler<,>).Name,
                    typeof(INotificationHandler<>).Name
                };

        foreach (var tipo in classesDaApplication)
        {
            var interfacesDosHandlers = tipo.ImplementedInterfaces
                .Select(i => i.GetTypeInfo())
                .Where(i => nomeDasInterfacesHandlers.Contains(i.Name));

            foreach (var interfaceDoHandler in interfacesDosHandlers)
            {
                services.AddTransient(interfaceDoHandler.AsType(), tipo.AsType());
            }
        }
    }

    internal static void LogPipelineBehavior(this IServiceCollection services)
        => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogPipelineBehavior<,>));

    internal static void ExceptionPipelineBehavior(this IServiceCollection services)
        => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionPipelineBehavior<,>));

    internal static void FailFastPipeline(this IServiceCollection services, ApiConfigurations fullConfig)
    {
        services.AddValidatorsFromAssembly(fullConfig.AssemblyType!.Assembly, includeInternalTypes: true);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
    internal static void PerformancePipelineBehavior(this IServiceCollection services)
        => services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

}