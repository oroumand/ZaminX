namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class ApplicationMetadataOptions
{
    public string? ApplicationName { get; set; }
    public string? ServiceName { get; set; }
    public string? ServiceVersion { get; set; }
    public string? ServiceInstanceId { get; set; }
}
