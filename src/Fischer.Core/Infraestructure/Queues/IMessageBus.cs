using EasyNetQ;
using Fischer.Core.Infraestructure.Queues.Integration;

namespace Fischer.Core.Infraestructure.Queues;
public interface IMessageBus : IDisposable
{
    bool IsConnected { get; }
    IAdvancedBus AdvancedBus { get; }
    void Publish<T>(T message) where T : IntegrationEvent;
    Task PublishAsync<T>(T message) where T : QueueInput;
    Task PublishInTimeAsync<T>(T input, TimeSpan delay) where T : IntegrationEvent;
    void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class;
    Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class;
    TResponse? Request<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage;
    Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage;
    IDisposable? Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage;
    Task<IDisposable> RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage;
}