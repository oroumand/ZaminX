using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Repositories;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Internals;

internal static class WriteRepositoryRegistrationHelper
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
                    IsEfWriteRepository(type))
                .ToList();

            foreach (var implementationType in implementationTypes)
            {
                var serviceTypes = implementationType
                    .GetInterfaces()
                    .Where(IsWriteRepositoryInterface)
                    .ToList();

                foreach (var serviceType in serviceTypes)
                {
                    RegisterScopedIfMissing(services, serviceType, implementationType);
                }
            }
        }
    }

    private static bool IsEfWriteRepository(Type type)
    {
        while (type.BaseType is not null)
        {
            var baseType = type.BaseType;

            if (baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(EfWriteRepository<,,>))
            {
                return true;
            }

            type = baseType;
        }

        return false;
    }

    private static bool IsWriteRepositoryInterface(Type type)
    {
        if (!type.IsInterface)
        {
            return false;
        }

        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IWriteRepository<,>))
        {
            return true;
        }

        return type.GetInterfaces()
            .Any(interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IWriteRepository<,>));
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
