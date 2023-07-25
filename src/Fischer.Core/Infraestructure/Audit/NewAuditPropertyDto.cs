namespace Fischer.Core.Infraestructure.Audit;
public sealed record NewAuditPropertyDto
{
    public string? PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public bool IsPrimaryKey { get; set; }
}