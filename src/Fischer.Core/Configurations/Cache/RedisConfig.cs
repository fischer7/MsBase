namespace Fischer.Core.Configurations.Cache;
public sealed class RedisConfig
{
    public string? Endpoint { get; set; }
    public int TTL { get; set; }
    public string? SessionName { get; set; }
    public string? InstanceName { get; set; }
}