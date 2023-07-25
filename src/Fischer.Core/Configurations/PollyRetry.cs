namespace Fischer.Core.Configurations;
public sealed record PollyRetry
{
    public int RetryCount { get; set; }
    public double DurationSecs { get; set; }
}