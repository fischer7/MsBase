using Fischer.Core.Domain.Records;

namespace Fischer.Core.Services.Statistics;
public interface IStatisticsService
{
    Task<bool> AddOneAsync(StatisticsRecord statisticsRecord, CancellationToken cancellationToken);
}