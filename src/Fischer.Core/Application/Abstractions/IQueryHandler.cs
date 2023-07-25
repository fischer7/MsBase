using Fischer.Core.Domain.Shared;
using MediatR;

namespace Fischer.Core.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}