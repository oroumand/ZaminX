using FluentValidation;
using System.Reflection;
using ZaminX.BuildingBlocks.Application.FluentValidation.Configurations;
using ZaminX.BuildingBlocks.Application.FluentValidation.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Mediation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXApplicationFluentValidation(
        this IServiceCollection services,
        Action<FluentValidationConfiguration>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var configuration = new FluentValidationConfiguration();
        configure?.Invoke(configuration);

        RegisterValidators(services, configuration.Assemblies);

        services.AddScoped(
            typeof(IMessageBehavior<,>),
            typeof(FluentValidationBehavior<,>));

        return services;
    }

    private static void RegisterValidators(
        IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        var distinctAssemblies = assemblies
            .Where(x => x is not null)
            .Distinct()
            .ToArray();

        foreach (var assembly in distinctAssemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }
    }
}