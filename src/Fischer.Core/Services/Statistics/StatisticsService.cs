using Fischer.Core.Configurations;
using Fischer.Core.Configurations.API;
using Fischer.Core.Constants;
using Fischer.Core.Domain.Records;
using System.Collections.Concurrent;

namespace Fischer.Core.Services.Statistics;
public sealed class StatisticsService : IStatisticsService
{
    private static ConcurrentBag<StatisticsRecord> StatisticsRecords = new();
    private readonly int StatisticBagLimit = 5000;
    private readonly int StatisticBagCleanUpPercentage = 10;
    private readonly int _statisticBagSkip;

    public StatisticsService(ApiConfigurations configurations)
    {
        StatisticBagLimit = configurations.StatisticBagLimit;
        _statisticBagSkip = StatisticBagLimit * (1 - (StatisticBagCleanUpPercentage / 100));
    }

    public async Task<bool> AddOneAsync(StatisticsRecord statisticsRecord, CancellationToken cancellationToken)
    {
        await Task.Run(() => { StatisticsRecords.Add(statisticsRecord); });
        await ControlSize();
        return true;
    }
    private async Task ControlSize()
    {
        await Task.Run(() =>
            {
                if (StatisticsRecords.Count > StatisticBagLimit)
                {
                    StatisticsRecords = new ConcurrentBag<StatisticsRecord>(
                        StatisticsRecords.OrderBy(x => x.TimeOffset)
                        .Skip(_statisticBagSkip));
                }
            }
        );
    }
}
