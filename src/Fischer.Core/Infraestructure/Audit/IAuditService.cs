namespace Fischer.Core.Infraestructure.Audit;
public interface IAuditService
{
    Task HandleAuditAsync(List<NewAuditDto> audits);
}