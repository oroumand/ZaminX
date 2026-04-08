using Serilog.Core;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Builders;

public interface IZaminXLoggingBuilder
{
    IServiceCollection Services { get; }
    ZaminXLoggingBuilder UseConsole();
    ZaminXLoggingBuilder UseFile(Action<Configurations.FileSinkOptions>? configure = null);
    ZaminXLoggingBuilder UseSeq(string serverUrl, Action<Configurations.SeqSinkOptions>? configure = null);
    ZaminXLoggingBuilder WithMachineName(bool enabled = true);
    ZaminXLoggingBuilder WithEnvironmentName(bool enabled = true);
    ZaminXLoggingBuilder WithThreadId(bool enabled = true);
    ZaminXLoggingBuilder WithCorrelationId(Action<Configurations.CorrelationIdOptions>? configure = null);
    ZaminXLoggingBuilder WithTraceAndSpan(Action<Configurations.TraceEnricherOptions>? configure = null);
    ZaminXLoggingBuilder WithApplicationMetadata(Action<Configurations.ApplicationMetadataOptions>? configure = null);
    ZaminXLoggingBuilder WithEnricher<T>() where T : class, ILogEventEnricher;
    ZaminXLoggingBuilder WithEnrichers(params Type[] enricherTypes);
    ZaminXLoggingBuilder UseRequestLogging(Action<Configurations.RequestLoggingOptions>? configure = null);
}
