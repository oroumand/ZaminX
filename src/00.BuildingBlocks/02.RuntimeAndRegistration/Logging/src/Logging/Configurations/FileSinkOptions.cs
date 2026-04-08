using Serilog;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class FileSinkOptions
{
    public bool Enabled { get; set; } = true;
    public string Path { get; set; } = "logs/log-.txt";
    public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;
    public bool Shared { get; set; }
    public long? FileSizeLimitBytes { get; set; }
    public int? RetainedFileCountLimit { get; set; } = 31;
}
