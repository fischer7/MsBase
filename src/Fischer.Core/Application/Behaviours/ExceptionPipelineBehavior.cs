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
            _logger.LogError(ex.Message);
            var exceptionMessage = ex.Message
                    + (ex.InnerException is not null ? " - InnerException: " + ex.InnerException?.Message : string.Empty);

            return CreateExceptionResult<TResponse>(new Error("Exception", exceptionMessage));
        }

    }
    private static TResult CreateExceptionResult<TResult>(Error error) where TResult : Result
    {
        var errors = new List<Error> { error };
        object exceptionResult =
            typeof(ExceptionResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ExceptionResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)exceptionResult;
    }
}