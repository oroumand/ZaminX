using Serilog.Core;
using Serilog.Events;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Enrichers;

internal sealed class ApplicationMetadataEnricher(ApplicationMetadataOptions options) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        AddIfNotEmpty(logEvent, propertyFactory, nameof(options.ApplicationName), options.ApplicationName);
        AddIfNotEmpty(logEvent, propertyFactory, nameof(options.ServiceName), options.ServiceName);
        AddIfNotEmpty(logEvent, propertyFactory, nameof(options.ServiceVersion), options.ServiceVersion);
        AddIfNotEmpty(logEvent, propertyFactory, nameof(options.ServiceInstanceId), options.ServiceInstanceId);
    }

    private static void AddIfNotEmpty(LogEvent logEvent, ILogEventPropertyFactory propertyFactory, string propertyName, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(propertyName, value));
    }
}
