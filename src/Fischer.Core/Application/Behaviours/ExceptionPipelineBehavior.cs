//https://codewithmukesh.com/blog/mediatr-pipeline-behaviour/
using Fischer.Core.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fischer.Core.Application.Behaviours;
public class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> _logger;

    public ExceptionPipelineBehavior(ILogger<ExceptionPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception captured, {ex}", ex);
            return (ExceptionResult.WithErrors(new List<Error>()
                                { new Error("Exception", ex.Message) }) as TResponse)!;
        }
    }
}