using Fischer.Core.Infraestructure.Audit;

namespace Fischer.Core.Infraestructure.Queues.NewAudit;
public sealed class NewAuditQueueInput : QueueInput
{
    public NewAuditQueueInput(IEnumerable<NewAuditDto> audits)
    {
        Audits = audits ?? new List<NewAuditDto>();
    }

    public IEnumerable<NewAuditDto> Audits { get; set; }
}