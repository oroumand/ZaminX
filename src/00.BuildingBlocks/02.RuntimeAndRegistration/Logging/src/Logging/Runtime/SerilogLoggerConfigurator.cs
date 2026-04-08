using Serilog;
using Serilog.Events;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

internal static class SerilogLoggerConfigurator
{
    public static LoggerConfiguration CreateBaseConfiguration(IConfiguration configuration, ZaminXLoggingOptions options)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext();

        ApplyConfiguredSinks(loggerConfiguration, options);
        ApplyConfiguredEnrichers(loggerConfiguration, options);

        return loggerConfiguration;
    }

    public static void Configure(
        LoggerConfiguration loggerConfiguration,
        IConfiguration configuration,
        IServiceProvider services,
        ZaminXLoggingOptions options)
    {
        loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext();

        ApplyConfiguredSinks(loggerConfiguration, options);
        ApplyConfiguredEnrichers(loggerConfiguration, options);
    }

    private static void ApplyConfiguredSinks(LoggerConfiguration loggerConfiguration, ZaminXLoggingOptions options)
    {
        if (options.Sinks.Console.Enabled)
        {
            loggerConfiguration.WriteTo.Console();
        }

        if (options.Sinks.File.Enabled)
        {
            loggerConfiguration.WriteTo.File(
                path: options.Sinks.File.Path,
                rollingInterval: options.Sinks.File.RollingInterval,
                shared: options.Sinks.File.Shared,
                fileSizeLimitBytes: options.Sinks.File.FileSizeLimitBytes,
                retainedFileCountLimit: options.Sinks.File.RetainedFileCountLimit);
        }

        if (options.Sinks.Seq.Enabled && string.IsNullOrWhiteSpace(options.Sinks.Seq.ServerUrl) is false)
        {
            loggerConfiguration.WriteTo.Seq(options.Sinks.Seq.ServerUrl!, apiKey: options.Sinks.Seq.ApiKey);
        }
    }

    private static void ApplyConfiguredEnrichers(LoggerConfiguration loggerConfiguration, ZaminXLoggingOptions options)
    {
        if (options.Enrichers.UseMachineName)
        {
            loggerConfiguration.Enrich.WithMachineName();
        }

        if (options.Enrichers.UseEnvironmentName)
        {
            loggerConfiguration.Enrich.WithEnvironmentName();
        }

        if (options.Enrichers.UseThreadId)
        {
            loggerConfiguration.Enrich.WithThreadId();
        }
    }
}
