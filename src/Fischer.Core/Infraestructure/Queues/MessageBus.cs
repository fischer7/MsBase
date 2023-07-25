//Eduardo Pires Project: DevStore
using EasyNetQ;
using Fischer.Core.Configurations;
using Fischer.Core.Constants;
using Fischer.Core.Infraestructure.Queues.Integration;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace Fischer.Core.Infraestructure.Queues;
public class MessageBus : IMessageBus
{
    private IBus? _bus;

    private readonly string _connectionString;
    private readonly AppsettingsGet _appSettings;

    public MessageBus(AppsettingsGet appSettings)
    {
        _appSettings = appSettings;
        _connectionString = appSettings["RabbitMq:ConnectionString"] ?? string.Empty;

        TryConnect();
    }

    public bool IsConnected => _bus?.Advanced.IsConnected ?? false;
    public IAdvancedBus AdvancedBus => _bus?.Advanced!;
    public void Publish<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        _bus?.PubSub.Publish(message);
    }

    public async Task PublishAsync<T>(T message) where T : QueueInput
    {
        TryConnect();
        await (_bus?.PubSub.PublishAsync(message) ?? Task.CompletedTask);
    }

    public async Task PublishInTimeAsync<T>(T input, TimeSpan delay) where T : IntegrationEvent
    {
        TryConnect();
        await (_bus?.Scheduler.FuturePublishAsync(input, delay) ?? Task.CompletedTask);
    }

    public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
    {
        TryConnect();
        _bus?.PubSub.Subscribe(subscriptionId, onMessage);
    }

    public async Task SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
    {
        TryConnect();
        await (_bus?.PubSub.SubscribeAsync(subscriptionId, onMessage) ?? Task.CompletedTask);
    }

    public TResponse? Request<TRequest, TResponse>(TRequest request) where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus?.Rpc.Request<TRequest, TResponse>(request);
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return await _bus!.Rpc.RequestAsync<TRequest, TResponse>(request, x => x.WithExpiration(TimeSpan.FromMinutes(5)));
    }

    public IDisposable? Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return _bus?.Rpc.Respond(responder);
    }

    public async Task<IDisposable> RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        where TRequest : IntegrationEvent where TResponse : ResponseMessage
    {
        TryConnect();
        return await _bus!.Rpc.RespondAsync(responder);
    }

    private void TryConnect()
    {
        if (IsConnected) return;

        var pollyMqRetry = _appSettings.GetConfig<PollyRetry>();

        var policy = Policy.Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(pollyMqRetry?.RetryCount ?? CoreConstants.MqDefaultRetry, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(pollyMqRetry?.DurationSecs ?? CoreConstants.MqDurationSeconds, retryAttempt)));

        policy.Execute(() =>
        {
            _bus = RabbitHutch.CreateBus(_connectionString);
            AdvancedBus.Disconnected += OnDisconnect;
        });
    }

    private void OnDisconnect(object? s, EventArgs? e)
    {
        var policy = Policy.Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .RetryForever();

        policy.Execute(TryConnect);
    }

    public void Dispose()
    {
        _bus?.Dispose();
    }
}