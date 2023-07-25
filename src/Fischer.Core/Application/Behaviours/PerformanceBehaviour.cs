using Fischer.Core.Application.BlurData;
using Fischer.Core.Configurations.API;
using Fischer.Core.Domain.Records;
using Fischer.Core.Services.Statistics;
using Fischer.Core.Services.User;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Fischer.Core.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;
    private readonly IAspNetUser _currentUserService;
    private readonly ApiConfigurations _apiConfigurations;
    private readonly IStatisticsService _statisticsService;

    public PerformanceBehaviour(
        ILogger<TRequest> logger,
        IAspNetUser currentUserService,
        ApiConfigurations apiConfigurations,
        IStatisticsService statisticsService
        )
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
        _apiConfigurations = apiConfigurations;
        _statisticsService = statisticsService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_apiConfigurations.EnableMediatrPerformancePipeline)
            _timer.Start();

        var response = await next();

        if (_apiConfigurations.EnableMediatrPerformancePipeline)
        {
            _timer.Stop();

            var userId = _currentUserService.GetUserId();
            
            if(HideData.HasSensitiveData(typeof(TRequest)))
                HideData.HideSensitiveData(request);
            
            var statisticRecord = new StatisticsRecord(request.GetType().Name, DateTimeOffset.UtcNow, _timer.Elapsed,
                                        request, userId);
            
            await _statisticsService.AddOneAsync(statisticRecord, cancellationToken);
        }

        return response;
    }

}
