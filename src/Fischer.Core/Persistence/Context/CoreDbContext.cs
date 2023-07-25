using Fischer.Core.Configurations;
using Fischer.Core.Configurations.API;
using Fischer.Core.Constants;
using Fischer.Core.Extensions;
using Fischer.Core.Infraestructure.Audit;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Fischer.Core.Persistence.Context;
public abstract class CoreDbContext : DbContext, IUnitOfWork
{
    private readonly AppsettingsGet _appSettings;
    private readonly IAuditService _auditService;
    private readonly ApiConfigurations _apiConfigurations;

    protected CoreDbContext(
        AppsettingsGet appSettings,
        IAuditService auditService,
        ApiConfigurations apiConfigurations)
    {
        _appSettings = appSettings;
        _auditService = auditService;
        _apiConfigurations = apiConfigurations;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<ValidationResult>();

        foreach (var property in GetStringProperties(modelBuilder))
            property.SetColumnType("varchar(250)");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.ApplyConfigurationsFromAssembly(_apiConfigurations.AssemblyType!.Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_appSettings == null) return;

        var connectionString = _appSettings[CoreConstants.ConnectionStringKey];

        optionsBuilder.UseNpgsql(connectionString);
    }

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = new())
    {
        FillLocalAudits();
        var audits = _apiConfigurations.EnableAudits ? ChangeTracker.GetAuditEvents() : new List<NewAuditDto>();

        var success = await SaveChangesAsync(cancellationToken) > 0;

        if (!success) return false;

        if (audits.Any())
            await _auditService.HandleAuditAsync(audits);

        return true;
    }

    private void FillLocalAudits()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Added)
                FillCreation(entry);
            else if (entry.State is EntityState.Modified)
                FillAlterDate(entry);
        }
    }

    private static void FillCreation(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        if (entry.Entity.GetType().GetProperty("CreationDate") != null)
            entry.Property("CreationDate").CurrentValue = DateTime.UtcNow;
    }

    private static void FillAlterDate(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
    {
        if (entry.Entity.GetType().GetProperty("UpdateDate") != null)
            entry.Property("UpdateDate").CurrentValue = DateTime.UtcNow;
    }

    private static IEnumerable<IMutableProperty> GetStringProperties(ModelBuilder modelBuilder)
    {
        return modelBuilder.Model.GetEntityTypes().SelectMany(e =>
            e.GetProperties().Where(p => p.ClrType == typeof(string)));
    }
}