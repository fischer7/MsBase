using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using Fischer.Core.Constants;
using Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;

namespace Fischer.Core.Infraestructure.Queues;
public sealed class EmailBus : IEmailBus
{
    private readonly IMessageBus _messageBus;
    private readonly IAdvancedBus _advancedBus;
    private readonly Exchange _exchange;
    private readonly Queue _emailQueue;

    public EmailBus(IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _advancedBus = messageBus.AdvancedBus;

        _exchange = _advancedBus.ExchangeDeclare("Email", ExchangeType.Direct);
        _emailQueue = _advancedBus.QueueDeclare("Emails.Envio", true, false, false);

        _advancedBus.Bind(_exchange, _emailQueue, CoreConstants.EmailRoutingKey);
    }

    public async Task Publish<TModel>(TModel input) where TModel : EmailInput
    {
        var emailMessage = new Message<TModel>(input);

        await _advancedBus.PublishAsync(_exchange, CoreConstants.EmailRoutingKey, true, emailMessage);
    }

    public void Consume(Action<IHandlerRegistration> addHandlers)
    {
        _advancedBus.Consume(_emailQueue, addHandlers, c =>
        {
            c.WithPrefetchCount(2);
        });
    }

    public void ConsumeRetry(Func<EmailIdQueueInput, Task> onMessage)
    {
        _messageBus.SubscribeAsync("Email.Retentativa.Consumer", onMessage);
    }
}