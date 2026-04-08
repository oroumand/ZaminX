using Serilog.Core;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Context;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Builders;

public sealed class ZaminXLoggingBuilder : IZaminXLoggingBuilder
{
    private readonly HashSet<Type> _enricherTypes = [];

    public ZaminXLoggingBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    public ZaminXLoggingOptions Options { get; } = new();

    public IReadOnlyCollection<Type> CustomEnricherTypes => _enricherTypes;

    public ZaminXLoggingBuilder UseConsole()
    {
        Options.Sinks.Console.Enabled = true;
        return this;
    }

    public ZaminXLoggingBuilder UseFile(Action<FileSinkOptions>? configure = null)
    {
        Options.Sinks.File.Enabled = true;
        configure?.Invoke(Options.Sinks.File);
        return this;
    }

    public ZaminXLoggingBuilder UseSeq(string serverUrl, Action<SeqSinkOptions>? configure = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serverUrl);
        Options.Sinks.Seq.Enabled = true;
        Options.Sinks.Seq.ServerUrl = serverUrl;
        configure?.Invoke(Options.Sinks.Seq);
        return this;
    }

    public ZaminXLoggingBuilder WithMachineName(bool enabled = true)
    {
        Options.Enrichers.UseMachineName = enabled;
        return this;
    }

    public ZaminXLoggingBuilder WithEnvironmentName(bool enabled = true)
    {
        Options.Enrichers.UseEnvironmentName = enabled;
        return this;
    }

    public ZaminXLoggingBuilder WithThreadId(bool enabled = true)
    {
        Options.Enrichers.UseThreadId = enabled;
        return this;
    }

    public ZaminXLoggingBuilder WithCorrelationId(Action<CorrelationIdOptions>? configure = null)
    {
        Options.Enrichers.CorrelationId.Enabled = true;
        configure?.Invoke(Options.Enrichers.CorrelationId);
        return this;
    }

    public ZaminXLoggingBuilder WithTraceAndSpan(Action<TraceEnricherOptions>? configure = null)
    {
        Options.Enrichers.Trace.Enabled = true;
        configure?.Invoke(Options.Enrichers.Trace);
        return this;
    }

    public ZaminXLoggingBuilder WithApplicationMetadata(Action<ApplicationMetadataOptions>? configure = null)
    {
        configure?.Invoke(Options.Application);
        return this;
    }

    public ZaminXLoggingBuilder WithEnricher<T>() where T : class, ILogEventEnricher
    {
        _enricherTypes.Add(typeof(T));
        return this;
    }

    public ZaminXLoggingBuilder WithEnrichers(params Type[] enricherTypes)
    {
        ArgumentNullException.ThrowIfNull(enricherTypes);
        foreach (var enricherType in enricherTypes)
        {
            ArgumentNullException.ThrowIfNull(enricherType);
            _enricherTypes.Add(enricherType);
        }
        return this;
    }

    public ZaminXLoggingBuilder UseRequestLogging(Action<RequestLoggingOptions>? configure = null)
    {
        Options.RequestLogging.Enabled = true;
        configure?.Invoke(Options.RequestLogging);
        return this;
    }
}
