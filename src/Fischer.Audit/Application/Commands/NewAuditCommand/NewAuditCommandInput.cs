using Fischer.Core.Application.Abstractions;
using Fischer.Core.Infraestructure.Audit;

namespace Fischer.Audit.Application.Commands.NewAuditCommand;
public sealed record NewAuditCommandInput : ICommand<NewAuditCommandResult>
{
    public IEnumerable<NewAuditDto>? Audits { get; init; }
}