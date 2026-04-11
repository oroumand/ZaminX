using System.Reflection;
using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Configurations;
using ZaminX.BuildingBlocks.Application.Events;
using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Queries;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXApplication(
        this IServiceCollection services,
        Action<RelayConfiguration>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var configuration = new RelayConfiguration();
        configure?.Invoke(configuration);

        services.AddScoped<IMediator, Mediator>();

        RegisterBuiltInBehaviors(services, configuration);
        RegisterCustomBehaviors(services, configuration);

        return services;
    }

    public static IServiceCollection AddZaminXApplicationHandlers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        RegisterHandlers(services, assemblies);

        return services;
    }

    public static IServiceCollection AddZaminXApplicationHandlers(
        this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        RegisterHandlers(services, assemblies);

        return services;
    }

    private static void RegisterBuiltInBehaviors(
        IServiceCollection services,
        RelayConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        if (configuration.EnableRequestTelemetryBehavior)
        {
            services.AddScoped(
                typeof(IMessageBehavior<,>),
                typeof(RequestTelemetryBehavior<,>));
        }

        if (configuration.EnableValidationBehavior)
        {
            services.AddScoped(
                typeof(IMessageBehavior<,>),
                typeof(ValidationBehavior<,>));
        }

        if (configuration.EnableExceptionToResultBehavior)
        {
            services.AddScoped(
                typeof(IMessageBehavior<,>),
                typeof(ExceptionToResultBehavior<,>));
        }
    }

    private static void RegisterCustomBehaviors(
        IServiceCollection services,
        RelayConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (var openBehaviorType in configuration.OpenBehaviorTypes)
        {
            services.AddScoped(typeof(IMessageBehavior<,>), openBehaviorType);
        }
    }

    private static void RegisterHandlers(
        IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        var distinctAssemblies = assemblies
            .Where(x => x is not null)
            .Distinct()
            .ToArray();

        if (distinctAssemblies.Length == 0)
            return;

        var implementationTypes = distinctAssemblies
            .SelectMany(GetLoadableTypes)
            .Where(type =>
                type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false })
            .ToArray();

        foreach (var implementationType in implementationTypes)
        {
            var serviceTypes = implementationType
                .GetInterfaces()
                .Where(IsSupportedHandlerInterface)
                .ToArray();

            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType, implementationType);
            }
        }
    }

    private static bool IsSupportedHandlerInterface(Type interfaceType)
    {
        if (!interfaceType.IsGenericType)
            return false;

        var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

        return genericTypeDefinition == typeof(ICommandHandler<>)
               || genericTypeDefinition == typeof(ICommandHandler<,>)
               || genericTypeDefinition == typeof(IQueryHandler<,>)
               || genericTypeDefinition == typeof(IEventHandler<>);
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types.Where(type => type is not null)!;
        }
    }
}