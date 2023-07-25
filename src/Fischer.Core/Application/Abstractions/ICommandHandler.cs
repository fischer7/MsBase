using Fischer.Core.Domain.Shared;
using MediatR;

namespace Fischer.Core.Application.Abstractions;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
