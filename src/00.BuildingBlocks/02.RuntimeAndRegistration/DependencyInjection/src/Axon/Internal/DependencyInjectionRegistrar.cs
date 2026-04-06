
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Abstractions.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Abstractions.Models;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Exceptions;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Internal;
internal static class DependencyInjectionRegistrar
{
    private static readonly Type[] LifetimeMarkers =
    [
        typeof(ITransientDependency),
        typeof(IScopedDependency),
        typeof(ISingletonDependency)
    ];

    public static void Register(
        IServiceCollection services,
        DependencyInjectionRegistrationOptions options)
    {
        foreach (var implementationType in GetCandidateTypes(options))
        {
            var registration = BuildRegistration(implementationType, options);

            if (registration is null)
            {
                continue;
            }

            foreach (var serviceType in registration.ServiceTypes)
            {
                AddDescriptor(
                    services,
                    serviceType,
                    implementationType,
                    registration.Lifetime,
                    options.DuplicateRegistrationBehavior);
            }
        }
    }

    private static IReadOnlyCollection<Type> GetCandidateTypes(DependencyInjectionRegistrationOptions options)
    {
        return [.. options.Assemblies
            .Except(options.ExcludedAssemblies)
            .SelectMany(GetLoadableTypes)
            .Where(type => type is { IsClass: true, IsAbstract: false, IsNestedPrivate: false })
            .Where(type => options.ExcludedTypes.Contains(type) is false)
            .Where(type => IsExcludedByNamespace(type, options) is false)
            .Where(type => options.TypeFilter?.Invoke(type) ?? true)];
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

    private static bool IsExcludedByNamespace(Type type, DependencyInjectionRegistrationOptions options)
    {
        if (type.Namespace is null)
        {
            return false;
        }

        return options.ExcludedNamespacePrefixes.Any(type.Namespace.StartsWith);
    }

    private static ServiceRegistration? BuildRegistration(
        Type implementationType,
        DependencyInjectionRegistrationOptions options)
    {
        var markerLifetime = GetLifetimeFromMarker(implementationType);

        if (markerLifetime is not null && options.EnableMarkerRegistration)
        {
            var markerServiceTypes = GetServiceTypesFromMarkers(implementationType, options);
            return markerServiceTypes.Count == 0
                ? null
                : new ServiceRegistration(markerServiceTypes, markerLifetime.Value);
        }

        if (options.EnableConventionRegistration is false)
        {
            return null;
        }

        var conventionalServiceTypes = GetServiceTypesFromConvention(implementationType, options);
        return conventionalServiceTypes.Count == 0
            ? null
            : new ServiceRegistration(conventionalServiceTypes, options.ConventionalLifetime);
    }

    private static ServiceLifetime? GetLifetimeFromMarker(Type implementationType)
    {
        var interfaces = implementationType.GetInterfaces();
        var hasTransient = interfaces.Contains(typeof(ITransientDependency));
        var hasScoped = interfaces.Contains(typeof(IScopedDependency));
        var hasSingleton = interfaces.Contains(typeof(ISingletonDependency));
        var markerCount = new[] { hasTransient, hasScoped, hasSingleton }.Count(x => x);

        if (markerCount > 1)
        {
            throw new ZaminXDependencyInjectionException(
                $"Type '{implementationType.FullName}' implements more than one lifetime marker.");
        }

        if (hasTransient)
        {
            return ServiceLifetime.Transient;
        }

        if (hasScoped)
        {
            return ServiceLifetime.Scoped;
        }

        if (hasSingleton)
        {
            return ServiceLifetime.Singleton;
        }

        return null;
    }

    private static IReadOnlyCollection<Type> GetServiceTypesFromMarkers(
        Type implementationType,
        DependencyInjectionRegistrationOptions options)
    {
        var interfaces = implementationType
            .GetInterfaces()
            .Where(interfaceType => LifetimeMarkers.Contains(interfaceType) is false)
            .ToArray();

        if (interfaces.Length > 0)
        {
            return NormalizeServiceTypes(implementationType, interfaces, options);
        }

        return options.RegisterSelfWhenNoServiceTypeFound
            ? [implementationType]
            : [];
    }

    private static IReadOnlyCollection<Type> GetServiceTypesFromConvention(
        Type implementationType,
        DependencyInjectionRegistrationOptions options)
    {
        var expectedInterfaceName = $"I{implementationType.Name}";
        var interfaces = implementationType
            .GetInterfaces()
            .Where(interfaceType => interfaceType.Name.Equals(expectedInterfaceName, StringComparison.Ordinal))
            .ToArray();

        if (interfaces.Length > 0)
        {
            return NormalizeServiceTypes(implementationType, interfaces, options);
        }

        return options.RegisterSelfWhenNoServiceTypeFound
            ? [implementationType]
            : [];
    }

    private static IReadOnlyCollection<Type> NormalizeServiceTypes(
        Type implementationType,
        Type[] serviceTypes,
        DependencyInjectionRegistrationOptions options)
    {
        return [.. serviceTypes
            .Where(serviceType => options.RegisterOpenGenerics || (serviceType.IsGenericTypeDefinition is false && implementationType.IsGenericTypeDefinition is false))
            .Select(serviceType => serviceType.IsGenericType ? serviceType.GetGenericTypeDefinitionOrSelf() : serviceType)
            .Distinct()];
    }

    private static void AddDescriptor(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime,
        DuplicateRegistrationBehavior behavior)
    {
        var normalizedImplementationType = implementationType.IsGenericType
            ? implementationType.GetGenericTypeDefinitionOrSelf()
            : implementationType;

        var descriptor = new ServiceDescriptor(serviceType, normalizedImplementationType, lifetime);
        var existingDescriptors = services
            .Where(existing => existing.ServiceType == serviceType)
            .ToArray();

        switch (behavior)
        {
            case DuplicateRegistrationBehavior.Skip:
                services.TryAdd(descriptor);
                break;

            case DuplicateRegistrationBehavior.Replace:
                foreach (var existing in existingDescriptors)
                {
                    services.Remove(existing);
                }

                services.Add(descriptor);
                break;

            case DuplicateRegistrationBehavior.Throw:
                if (existingDescriptors.Length > 0)
                {
                    throw new ZaminXDependencyInjectionException(
                        $"Duplicate registration detected for service type '{serviceType.FullName}'.");
                }

                services.Add(descriptor);
                break;

            default:
                throw new ZaminXDependencyInjectionException(
                    $"Unsupported duplicate registration behavior '{behavior}'.");
        }
    }

    private static Type GetGenericTypeDefinitionOrSelf(this Type type)
        => type.IsGenericType ? type.GetGenericTypeDefinition() : type;

    private sealed record ServiceRegistration(IReadOnlyCollection<Type> ServiceTypes, ServiceLifetime Lifetime);
}
