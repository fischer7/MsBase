using MediatR;
using Fischer.Core.Domain.Shared;

namespace Fischer.Core.Application.Abstractions;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }