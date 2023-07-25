using Fischer.Audit.Domain;
using Fischer.Audit.Persistence.Repositories.Interfaces;
using Fischer.Core.Application.Abstractions;
using Fischer.Core.Domain.Shared;
using Fischer.Core.Infraestructure.Audit;

namespace Fischer.Audit.Application.Commands.NewAuditCommand;
public sealed class NewAuditCommandHandler : ICommandHandler<NewAuditCommandInput, NewAuditCommandResult>
{
    private readonly IAuditRepository _auditRepository;

    public NewAuditCommandHandler(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task<Result<NewAuditCommandResult>> Handle(NewAuditCommandInput command, CancellationToken cancellationToken)
    {
        foreach (var auditCommand in command.Audits ?? Enumerable.Empty<NewAuditDto>())
        {
            var audit = new Domain.Audit
            {
                UserId = auditCommand.UserId,
                EntityId = auditCommand.EntityId,
                EntityName = auditCommand.EntityName,
                TableName = auditCommand.TableName,
                AuditKind = auditCommand.AuditKind,
                Props = auditCommand.Properties?.Select(c => new AuditProp
                {
                    IsPrimaryKey = c.IsPrimaryKey,
                    PropertyName = c.PropertyName,
                    OldValue = c.OldValue,
                    NewValue = c.NewValue,
                })?.ToList()
            };

            _auditRepository.Add(audit);
        }

        await _auditRepository.UnitOfWork.CommitAsync(cancellationToken);

        return new NewAuditCommandResult();
    }
}