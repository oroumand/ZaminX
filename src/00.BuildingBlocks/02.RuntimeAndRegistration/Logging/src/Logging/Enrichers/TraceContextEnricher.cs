using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Enrichers;

internal sealed class TraceContextEnricher(TraceEnricherOptions options) : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        if (options.Enabled is false)
        {
            return;
        }

        var activity = Activity.Current;
        if (activity is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(options.TraceIdPropertyName) is false)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(options.TraceIdPropertyName, activity.TraceId.ToString()));
        }

        if (string.IsNullOrWhiteSpace(options.SpanIdPropertyName) is false)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(options.SpanIdPropertyName, activity.SpanId.ToString()));
        }
    }
}
