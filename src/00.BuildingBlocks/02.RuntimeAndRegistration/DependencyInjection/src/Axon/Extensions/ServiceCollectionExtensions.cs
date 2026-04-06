using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Exceptions;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Internal;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXDependencyInjection(
        this IServiceCollection services,
        Action<DependencyInjectionRegistrationOptions> setupAction)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(setupAction);

        var options = new DependencyInjectionRegistrationOptions();
        setupAction(options);
        Validate(options);

        DependencyInjectionRegistrar.Register(services, options);

        return services;
    }

    private static void Validate(DependencyInjectionRegistrationOptions options)
    {
        if (options.Assemblies.Count == 0)
        {
            throw new ZaminXDependencyInjectionException(
                "At least one assembly must be provided for dependency injection scanning.");
        }

        if (options.EnableMarkerRegistration is false && options.EnableConventionRegistration is false)
        {
            throw new ZaminXDependencyInjectionException(
                "At least one registration strategy must be enabled. Marker-based or convention-based registration must be active.");
        }
    }
}
