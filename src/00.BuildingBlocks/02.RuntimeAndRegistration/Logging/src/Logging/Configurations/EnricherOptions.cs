namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class EnricherOptions
{
    public bool UseMachineName { get; set; } = true;
    public bool UseEnvironmentName { get; set; } = true;
    public bool UseThreadId { get; set; }
    public CorrelationIdOptions CorrelationId { get; set; } = new();
    public TraceEnricherOptions Trace { get; set; } = new();
}
