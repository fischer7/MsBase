//https://codewithmukesh.com/blog/audit-trail-implementation-in-aspnet-core/
using Fischer.Core.Domain;
using Fischer.Core.Domain.Primitives;

namespace Fischer.Audit.Domain;
public class AuditProp : Entity, IDontAudit
{
    public string? PropertyName { get; init; }
    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    public bool? IsPrimaryKey { get; init; }
    public Guid? AuditId { get; private set; }
    public virtual Audit? Audit { get; private set; }
}