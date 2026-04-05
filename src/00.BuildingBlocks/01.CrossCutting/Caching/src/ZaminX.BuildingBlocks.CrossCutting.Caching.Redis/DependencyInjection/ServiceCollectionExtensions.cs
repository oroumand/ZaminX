namespace Microsoft.Extensions.DependencyInjection;

using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Redis.Options;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Redis.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXCachingWithRedis(
        this IServiceCollection services,
        Action<RedisCachingOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var redisCachingOptions = new RedisCachingOptions();
        configure(redisCachingOptions);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCachingOptions.Configuration;
            options.InstanceName = redisCachingOptions.InstanceName;
        });

        services.AddSingleton<IStashX, RedisStashX>();

        return services;
    }
}