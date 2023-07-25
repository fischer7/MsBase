using EasyNetQ;
using EasyNetQ.Consumer;
using Fischer.Core.Infraestructure.Queues;
using Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;

namespace Fischer.Email.Services;
public sealed class EmailQueueConsumer : BackgroundService
{
    private readonly IEmailBus _emailBus;
    private readonly ILogger<EmailQueueConsumer> _logger;

    public EmailQueueConsumer(IEmailBus emailBus,
        ILogger<EmailQueueConsumer> logger)
    {
        _emailBus = emailBus;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _emailBus.Consume(x =>
            x.Add<UserSuccessfullyRegisteredEmailInput>(UserRegistered)
             );

        return Task.CompletedTask;
    }

    private async Task<AckStrategy> UserRegistered(IMessage<UserSuccessfullyRegisteredEmailInput> message,
        MessageReceivedInfo receivedInfo, CancellationToken cancellationToken)
    {
        await Task.Run(() => _logger.LogInformation("Email queue received. {Message}.",message));
        //TODO: Send email notification
        return AckStrategies.Ack;
    }

}
