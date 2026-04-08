using Serilog.Events;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

internal static class RequestLoggingConfigurator
{
    public static void Use(WebApplication app, RequestLoggingOptions options, CorrelationIdOptions correlationIdOptions)
    {
        app.UseSerilogRequestLogging(setup =>
        {
            setup.MessageTemplate = options.MessageTemplate;
            setup.GetLevel = static (httpContext, _, exception) => exception is not null || httpContext.Response.StatusCode >= 500 ? LogEventLevel.Error : LogEventLevel.Information;
            setup.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                if (options.IncludeQueryString && httpContext.Request.QueryString.HasValue)
                {
                    diagnosticContext.Set("QueryString", httpContext.Request.QueryString.Value!);
                }

                if (httpContext.Items.TryGetValue(ZaminXLoggingWebApplicationExtensions.CorrelationIdItemKey, out var correlationId) && correlationId is string correlationIdValue)
                {
                    diagnosticContext.Set(correlationIdOptions.PropertyName, correlationIdValue);
                }
            };
        });
    }
}
