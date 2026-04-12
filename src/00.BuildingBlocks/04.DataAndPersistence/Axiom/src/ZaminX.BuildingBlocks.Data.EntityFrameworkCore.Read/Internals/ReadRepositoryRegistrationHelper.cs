using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Internals;

internal static class ReadRepositoryRegistrationHelper
{
    public static void RegisterRepositories(
    IServiceCollection services,
    params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies.Distinct())
        {
            var implementationTypes = assembly
                .GetTypes()
                .Where(type =>
                    type is { IsClass: true, IsAbstract: false } &&
                    IsEfReadRepository(type))
                .ToList();

            foreach (var implementationType in implementationTypes)
            {
                var serviceTypes = implementationType
                    .GetInterfaces()
                    .Where(IsReadRepositoryInterface)
                    .ToList();

                foreach (var serviceType in serviceTypes)
                {
                    RegisterScopedIfMissing(services, serviceType, implementationType);
                }
            }
        }
    }

    private static bool IsEfReadRepository(Type type)
    {
        while (type.BaseType is not null)
        {
            var baseType = type.BaseType;

            if (baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(EfReadRepository<,,>))
            {
                return true;
            }

            type = baseType;
        }

        return false;
    }

    private static bool IsReadRepositoryInterface(Type type)
    {
        if (!type.IsInterface)
        {
            return false;
        }

        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IReadRepository<,>))
        {
            return true;
        }

        return type.GetInterfaces()
            .Any(interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IReadRepository<,>));
    }

    private static void RegisterScopedIfMissing(
        IServiceCollection services,
        Type serviceType,
        Type implementationType)
    {
        var alreadyRegistered = services.Any(service =>
            service.ServiceType == serviceType &&
            service.ImplementationType == implementationType);

        if (alreadyRegistered)
        {
            return;
        }

        services.AddScoped(serviceType, implementationType);
    }


}
