using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Enrichers;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

namespace Microsoft.Extensions.DependencyInjection;
public static class ZaminXLoggingWebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddZaminXLogging(
        this WebApplicationBuilder builder,
        Action<ZaminXLoggingBuilder>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var configuredOptions = new ZaminXLoggingOptions();
        builder.Configuration.GetSection(ZaminXLoggingOptions.DefaultSectionName).Bind(configuredOptions);

        var loggingBuilder = new ZaminXLoggingBuilder(builder.Services);
        Copy(configuredOptions, loggingBuilder.Options);

        configure?.Invoke(loggingBuilder);
        Validate(loggingBuilder.Options);

        builder.Services.AddSingleton(loggingBuilder.Options);
        IServiceCollection serviceCollection = builder.Services.AddSingleton<IOptions<ZaminXLoggingOptions>>(Options.Options.Create(loggingBuilder.Options));
        builder.Services.AddHttpContextAccessor();

        var applicationMetadataEnricher = new ApplicationMetadataEnricher(loggingBuilder.Options.Application);
        var traceContextEnricher = new TraceContextEnricher(loggingBuilder.Options.Enrichers.Trace);

        builder.Services.AddSingleton<ILogEventEnricher>(applicationMetadataEnricher);
        builder.Services.AddSingleton<ILogEventEnricher>(traceContextEnricher);

        foreach (var enricherType in loggingBuilder.CustomEnricherTypes)
        {
            ValidateEnricherType(enricherType);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(ILogEventEnricher), enricherType));
        }

        ZaminXSerilogBootstrapper.Initialize(loggingBuilder, builder.Configuration);

        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            SerilogLoggerConfigurator.Configure(
                loggerConfiguration,
                context.Configuration,
                services,
                loggingBuilder.Options);
        });

        return builder;
    }

    private static void Copy(ZaminXLoggingOptions source, ZaminXLoggingOptions target)
    {
        target.Enabled = source.Enabled;
        target.Application = source.Application ?? new ApplicationMetadataOptions();
        target.Enrichers = source.Enrichers ?? new EnricherOptions();
        target.Sinks = source.Sinks ?? new SinkOptions();
        target.RequestLogging = source.RequestLogging ?? new RequestLoggingOptions();
    }

    private static void Validate(ZaminXLoggingOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.Sinks.File.Enabled && string.IsNullOrWhiteSpace(options.Sinks.File.Path))
        {
            throw new InvalidOperationException("Logging:File:Path must not be empty when the file sink is enabled.");
        }

        if (options.Sinks.Seq.Enabled && string.IsNullOrWhiteSpace(options.Sinks.Seq.ServerUrl))
        {
            throw new InvalidOperationException("Logging:Seq:ServerUrl must not be empty when the Seq sink is enabled.");
        }

        if (options.Enrichers.CorrelationId.Enabled)
        {
            if (string.IsNullOrWhiteSpace(options.Enrichers.CorrelationId.HeaderName))
            {
                throw new InvalidOperationException("Logging:Enrichers:CorrelationId:HeaderName must not be empty when correlation id is enabled.");
            }

            if (string.IsNullOrWhiteSpace(options.Enrichers.CorrelationId.PropertyName))
            {
                throw new InvalidOperationException("Logging:Enrichers:CorrelationId:PropertyName must not be empty when correlation id is enabled.");
            }
        }
    }

    private static void ValidateEnricherType(Type enricherType)
    {
        ArgumentNullException.ThrowIfNull(enricherType);

        if (!typeof(ILogEventEnricher).IsAssignableFrom(enricherType))
        {
            throw new InvalidOperationException($"Type '{enricherType.FullName}' must implement {nameof(ILogEventEnricher)}.");
        }

        if (enricherType.IsAbstract || enricherType.IsInterface)
        {
            throw new InvalidOperationException($"Type '{enricherType.FullName}' must be a concrete enricher type.");
        }
    }
}
