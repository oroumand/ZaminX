namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class ZaminXLoggingOptions
{
    public const string DefaultSectionName = "Logging";

    public bool Enabled { get; set; } = true;

    public ApplicationMetadataOptions Application { get; set; } = new();

    public EnricherOptions Enrichers { get; set; } = new();

    public SinkOptions Sinks { get; set; } = new();

    public RequestLoggingOptions RequestLogging { get; set; } = new();
}
