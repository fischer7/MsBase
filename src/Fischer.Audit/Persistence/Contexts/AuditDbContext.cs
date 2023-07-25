using Fischer.Audit.Domain;
using Fischer.Core.Configurations;
using Fischer.Core.Configurations.API;
using Fischer.Core.Infraestructure.Audit;
using Fischer.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Audit.Persistence.Contexts;
public sealed class AuditDbContext : CoreDbContext
{
    public AuditDbContext(AppsettingsGet environment, IAuditService auditService, ApiConfigurations settings)
        : base(environment, auditService, settings)
    {
    }

    public DbSet<Domain.Audit> Audit { get; set; }
    public DbSet<AuditProp> AuditProps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Audit");
    }
}