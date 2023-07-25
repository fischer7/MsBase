namespace Fischer.Core.Configurations.API;
public sealed record ApiConfigurations
{
    public Type? AssemblyType { get; set; }
    public bool EnableAudits { get; set; }
    public bool EnableMediatrExceptionPipeline { get; set; }
    public bool EnableMediatrFailFastPipeline { get; set; }
    public bool EnableMediatrLogPipeline { get; set; }
    public bool EnableMediatrPerformancePipeline { get; set; }
    public bool RunMigrations { get; set; }
    public int StatisticBagLimit { get; set; }
    public bool UseKeyCloak { get; set; }
    public bool UseRabbitMq { get; set; }
    public bool UseRedis { get; set; }
    public bool UseApiNotifications { get; set; }

}
