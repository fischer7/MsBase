using Fischer.Core.Domain.Primitives;
using MediatR;

namespace Fischer.Core.Application.Abstractions;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
