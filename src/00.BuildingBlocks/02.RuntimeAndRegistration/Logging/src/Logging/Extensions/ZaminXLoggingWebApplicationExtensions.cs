using Serilog.Context;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Context;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

namespace Microsoft.AspNetCore.Builder;

public static class ZaminXLoggingWebApplicationExtensions
{
    internal const string CorrelationIdItemKey = "ZaminX.Logging.CorrelationId";

    public static WebApplication UseZaminXLogging(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var options = app.Services.GetRequiredService<ZaminXLoggingOptions>();

        app.Use(async (context, next) =>
        {
            var correlationOptions = options.Enrichers.CorrelationId;

            if (!correlationOptions.Enabled)
            {
                await next();
                return;
            }

            var headerName = correlationOptions.HeaderName;
            var correlationId = context.Request.Headers[headerName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationId) && correlationOptions.GenerateIfMissing)
            {
                correlationId = Guid.NewGuid().ToString("N");
            }

            if (string.IsNullOrWhiteSpace(correlationId))
            {
                await next();
                return;
            }

            context.Items[CorrelationIdItemKey] = correlationId;

            if (correlationOptions.AddToResponseHeaders)
            {
                context.Response.Headers[headerName] = correlationId;
            }

            using (LogContext.PushProperty(correlationOptions.PropertyName, correlationId))
            {
                await next();
            }
        });

        if (options.RequestLogging.Enabled)
        {
            RequestLoggingConfigurator.Use(app, options.RequestLogging, options.Enrichers.CorrelationId);
        }

        return app;
    }

    public static WebApplication UseZaminXLoggingContext(
        this WebApplication app,
        Action<ZaminXLoggingContextBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(configure);

        var builder = new ZaminXLoggingContextBuilder();
        configure(builder);
        var registrations = builder.Registrations.ToArray();

        app.Use(async (context, next) =>
        {
            if (registrations.Length == 0)
            {
                await next();
                return;
            }

            var disposables = new List<IDisposable>(registrations.Length);

            try
            {
                foreach (var registration in registrations)
                {
                    var value = registration.ValueFactory(context, context.RequestServices);
                    if (value is null)
                    {
                        continue;
                    }

                    disposables.Add(LogContext.PushProperty(registration.PropertyName, value));
                }

                await next();
            }
            finally
            {
                for (var i = disposables.Count - 1; i >= 0; i--)
                {
                    disposables[i].Dispose();
                }
            }
        });

        return app;
    }

    public static async Task RunWithZaminXLoggingAsync(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        try
        {
            Log.Information("Starting application");
            await app.RunAsync();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Application terminated unexpectedly");
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
