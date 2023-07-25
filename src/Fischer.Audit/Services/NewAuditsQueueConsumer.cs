using Fischer.Audit.Application.Commands.NewAuditCommand;
using Fischer.Core.Infraestructure.Queues;
using Fischer.Core.Infraestructure.Queues.NewAudit;
using MediatR;

namespace Fischer.Audit.Services;
public sealed class NewAuditsQueueConsumer : BackgroundService
{
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public NewAuditsQueueConsumer(IMessageBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.SubscribeAsync<NewAuditQueueInput>(
            nameof(NewAuditsQueueConsumer),
            RegisterAudits);
    }

    private async Task RegisterAudits(NewAuditQueueInput request)
    {
        using var scope = _serviceProvider.CreateScope();

        var command = new NewAuditCommandInput
        {
            Audits = request.Audits
        };

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var result = await mediator.Send(command);
    }
}