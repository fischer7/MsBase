//https://codewithmukesh.com/blog/audit-trail-implementation-in-aspnet-core/
using Fischer.Core.Domain;
using Fischer.Core.Domain.Primitives;
using Fischer.Core.Infraestructure.Audit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fischer.Core.Extensions;
public static class AuditExtensions
{
    public static List<NewAuditDto> GetAuditEvents(this ChangeTracker changeTracker)
    {
        var audits = changeTracker.Entries()
            .Where(e => e.State
                is EntityState.Added
                or EntityState.Modified
                or EntityState.Deleted &&
                e.Entity is (Entity
                    or IdentityUser<Guid>
                    or IdentityRole<Guid>
                    or IdentityUserRole<Guid>
                    or IdentityUserClaim<Guid>
                    or IdentityRoleClaim<Guid>)
                    and not IDontAudit)
            .Select(MapAudit)
            .ToList();

        return audits;
    }

    private static NewAuditDto MapAudit(EntityEntry entityEntry)
    {
        var audit = new NewAuditDto
        {
            EntityName = entityEntry.Metadata.Name,
            TableName = entityEntry.Metadata.GetTableName(),
            AuditKind = entityEntry.State,
        };
        ExtractAuditProps(entityEntry, audit);
        return audit;
    }

    private static void ExtractAuditProps(EntityEntry entityEntry, NewAuditDto audit)
    {
        var oldProperties = entityEntry.GetDatabaseValues();

        foreach (var property in entityEntry.Properties)
        {
            var isPrimaryKey = property.Metadata.IsPrimaryKey();
            if (isPrimaryKey)
            {
                audit.EntityId = property?.CurrentValue?.ToString();
            }

            if (audit.IsModified && PropertyValueDidntChanged(property, oldProperties))
            {
                continue;
            }

            var oldValue = GetOldValue(audit, oldProperties, property);
            var newValue = ObterValorNovo(audit, property);

            audit.Properties.Add(new NewAuditPropertyDto
            {
                IsPrimaryKey = isPrimaryKey,
                OldValue = oldValue,
                NewValue = newValue,
                PropertyName = property?.Metadata?.Name
            });
        }
    }

    private static bool PropertyValueDidntChanged(PropertyEntry? property, PropertyValues? propriedadesAntigas)
    {
        if (property?.Metadata is null)
            return false;

        var valorAntigo = propriedadesAntigas?[property.Metadata];

        return valorAntigo?.ToString() == property.CurrentValue?.ToString();
    }

    private static string? GetOldValue(NewAuditDto audit, PropertyValues? propriedadesAntigas,
        PropertyEntry? property)
    {
        if (property?.Metadata is null)
            return default;

        return !audit.IsAdded
            ? propriedadesAntigas?[property.Metadata]?.ToString()
            : default;
    }

    private static string? ObterValorNovo(NewAuditDto audit, PropertyEntry? property)
    {
        return !audit.IsDeleted
            ? property?.CurrentValue?.ToString()
            : null;
    }
}