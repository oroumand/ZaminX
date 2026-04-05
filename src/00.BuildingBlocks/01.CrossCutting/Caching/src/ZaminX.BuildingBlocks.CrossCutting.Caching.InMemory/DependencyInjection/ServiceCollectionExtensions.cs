namespace Microsoft.Extensions.DependencyInjection;

using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXCachingWithInMemory(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IStashX, InMemoryStashX>();

        return services;
    }
}