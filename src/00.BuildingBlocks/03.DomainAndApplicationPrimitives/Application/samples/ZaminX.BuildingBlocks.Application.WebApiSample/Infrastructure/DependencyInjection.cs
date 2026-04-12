using ZaminX.BuildingBlocks.Application.WebApiSample;
using ZaminX.BuildingBlocks.Application.WebApiSample.Behaviors;
using ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddRelaySample(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddHttpContextAccessor();

        services.AddSingleton<InMemoryOrderStore>();

        services.AddZaminXApplication(options =>
        {
            options.EnableRequestTelemetryBehavior = true;
            options.EnableExceptionToResultBehavior = true;

            options.AddOpenBehavior(typeof(HeaderLoggingBehavior<,>));
        });

        services.AddZaminXApplicationHandlers(typeof(SampleAssemblyMarker).Assembly);

        services.AddZaminXApplicationFluentValidation(options =>
        {
            options.AddAssembly(typeof(SampleAssemblyMarker).Assembly);
        });

        return services;
    }
}