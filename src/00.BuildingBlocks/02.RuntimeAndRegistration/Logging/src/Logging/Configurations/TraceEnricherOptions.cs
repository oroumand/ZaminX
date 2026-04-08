namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class TraceEnricherOptions
{
    public bool Enabled { get; set; } = true;
    public string TraceIdPropertyName { get; set; } = "TraceId";
    public string SpanIdPropertyName { get; set; } = "SpanId";
}
