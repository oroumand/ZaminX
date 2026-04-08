namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class RequestLoggingOptions
{
    public bool Enabled { get; set; } = true;
    public string MessageTemplate { get; set; } = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    public bool IncludeQueryString { get; set; }
}
