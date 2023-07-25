//https://codewithmukesh.com/blog/audit-trail-implementation-in-aspnet-core/
using Fischer.Core.Domain;
using Fischer.Core.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Audit.Domain;
public sealed class Audit : Entity, IDontAudit
{
    public Guid? UserId { get; set; }
    public EntityState AuditKind { get; init; }
    public string? EntityId { get; init; }
    public string? EntityName { get; init; }
    public string? TableName { get; init; }
    public ICollection<AuditProp>? Props { get; init; }
}
