using EasyNetQ.Consumer;
using Fischer.Core.Infraestructure.Queues.NewEmail.EmailInputs;

namespace Fischer.Core.Infraestructure.Queues;
public interface IEmailBus
{
    Task Publish<T>(T message) where T : EmailInput;
    void Consume(Action<IHandlerRegistration> addHandlers);
    void ConsumeRetry(Func<EmailIdQueueInput, Task> onMessage);
}