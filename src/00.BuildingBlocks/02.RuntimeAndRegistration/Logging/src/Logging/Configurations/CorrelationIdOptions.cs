namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class CorrelationIdOptions
{
    public bool Enabled { get; set; } = true;
    public string HeaderName { get; set; } = "X-Correlation-ID";
    public string PropertyName { get; set; } = "CorrelationId";
    public bool GenerateIfMissing { get; set; } = true;
    public bool AddToResponseHeaders { get; set; } = true;
}
