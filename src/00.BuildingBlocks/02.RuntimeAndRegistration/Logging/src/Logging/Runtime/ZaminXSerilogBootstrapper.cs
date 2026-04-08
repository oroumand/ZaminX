using Serilog;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Runtime;

internal static class ZaminXSerilogBootstrapper
{
    public static void Initialize(ZaminXLoggingBuilder builder, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        if (builder.Options.Enabled is false)
        {
            return;
        }

        Log.Logger = SerilogLoggerConfigurator
            .CreateBaseConfiguration(configuration, builder.Options)
            .CreateBootstrapLogger();
    }
}
