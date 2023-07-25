using Fischer.Core.Domain.Shared;
using MediatR;

namespace Fischer.Core.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}