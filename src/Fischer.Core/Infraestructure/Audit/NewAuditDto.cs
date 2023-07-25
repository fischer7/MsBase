using Microsoft.EntityFrameworkCore;

namespace Fischer.Core.Infraestructure.Audit;
public sealed record NewAuditDto
{
    public Guid? UserId { get; set; }

    public EntityState AuditKind { get; set; }

    public string? EntityId { get; set; }

    public string? EntityName { get; set; }

    public string? TableName { get; set; }

    public ICollection<NewAuditPropertyDto> Properties { get; set; }
        = new List<NewAuditPropertyDto>();

    public bool IsAdded => AuditKind == EntityState.Added;
    public bool IsModified => AuditKind == EntityState.Modified;
    public bool IsDeleted => AuditKind == EntityState.Deleted;
}