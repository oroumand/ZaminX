namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

public sealed class SinkOptions
{
    public ConsoleSinkOptions Console { get; set; } = new();
    public FileSinkOptions File { get; set; } = new();
    public SeqSinkOptions Seq { get; set; } = new();
}
