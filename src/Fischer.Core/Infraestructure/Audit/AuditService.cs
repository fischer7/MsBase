using Fischer.Core.Configurations.API;
using Fischer.Core.Infraestructure.Queues;
using Fischer.Core.Infraestructure.Queues.NewAudit;
using Fischer.Core.Services.User;

namespace Fischer.Core.Infraestructure.Audit;
public sealed class AuditService : IAuditService
{
    private readonly IAspNetUser _aspNetUser;
    private readonly IMessageBus _bus;
    private readonly ApiConfigurations _apiConfigurations;

    public AuditService(IAspNetUser aspNetUser, ApiConfigurations apiConfigurations, IMessageBus bus)
    {
        _aspNetUser = aspNetUser;
        _bus = bus;
        _apiConfigurations = apiConfigurations;
    }

    public async Task HandleAuditAsync(List<NewAuditDto> audits)
    {
        if (!_apiConfigurations.EnableAudits)
            return;

        var userIdStr = _aspNetUser.GetUserId();

        audits.ForEach(x => x.UserId = userIdStr);

        await _bus.PublishAsync(new NewAuditQueueInput(audits));
    }
}