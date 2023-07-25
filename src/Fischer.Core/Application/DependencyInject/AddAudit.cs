//https://codewithmukesh.com/blog/audit-trail-implementation-in-aspnet-core/
using Fischer.Core.Infraestructure.Audit;
using Microsoft.Extensions.DependencyInjection;

namespace Fischer.Core.Application.DependencyInject;
internal static class AddAudit
{
    internal static IServiceCollection AddAuditConfiguration(this IServiceCollection services)
    {
        services.AddTransient<IAuditService, AuditService>();

        return services;
    }
}