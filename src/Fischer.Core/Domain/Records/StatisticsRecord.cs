namespace Fischer.Core.Domain.Records;
    public record StatisticsRecord(string className, DateTimeOffset TimeOffset, TimeSpan? timeSpend, 
        object input, Guid? userId);