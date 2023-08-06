using Fischer.Core.Application.BlurData;
using Fischer.Core.Configurations.API;
using Fischer.Core.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fischer.Core.Application.Behaviours;
public class LogPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LogPipelineBehavior<TRequest, TResponse>> _logger;
    private readonly ApiConfigurations _apiConfigs;

    public LogPipelineBehavior(ILogger<LogPipelineBehavior<TRequest, TResponse>> logger, ApiConfigurations core)
    {
        _logger = logger;
        _apiConfigs = core;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (HideData.HasSensitiveData(typeof(TRequest)))
            _logger.LogInformation("Entering the Handler:{RequestType}, with sensitive data", typeof(TRequest).Name);
        else
            _logger.LogInformation("Entering the Handler: {RequestType} --> {@Request}", typeof(TRequest).Name, request);

        var result = await next();

        if (HideData.HasSensitiveData(typeof(TResponse)))
        {
            var jsonData = JsonConvert.SerializeObject(result);
            var tempResult = JsonConvert.DeserializeObject<TResponse>(jsonData);
            HideData.HideSensitiveData(tempResult);
            _logger.LogInformation("Out of the Handler {TipoRequest} --> {@Result}", typeof(TResponse).Name, tempResult);
        }
        else if (result?.IsSuccess ?? false)
            _logger.LogInformation("Out of the Handler {TipoRequest} --> {@Result}", typeof(TResponse).Name, result);
        else
            _logger.LogInformation("Fluent Fail at {RequestType}", typeof(TResponse));

        return result;
    }
}