namespace ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Services;

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Models;

public sealed class SqlServerStashX(IDistributedCache distributedCache) : IStashX
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task SetAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(value, SerializerOptions);
        var bytes = Encoding.UTF8.GetBytes(payload);
        var distributedCacheEntryOptions = CreateDistributedCacheEntryOptions(options);

        await distributedCache.SetAsync(
            key,
            bytes,
            distributedCacheEntryOptions,
            cancellationToken);
    }

    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        var bytes = await distributedCache.GetAsync(key, cancellationToken);

        if (bytes is null || bytes.Length == 0)
        {
            return default;
        }

        var payload = Encoding.UTF8.GetString(bytes);

        return JsonSerializer.Deserialize<T>(payload, SerializerOptions);
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var value = await factory(cancellationToken);

        await SetAsync(key, value, options, cancellationToken);

        return value;
    }

    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return distributedCache.RemoveAsync(key, cancellationToken);
    }

    private static DistributedCacheEntryOptions CreateDistributedCacheEntryOptions(CacheEntryOptions? options)
    {
        var distributedCacheEntryOptions = new DistributedCacheEntryOptions();

        if (options?.AbsoluteExpiration is not null)
        {
            distributedCacheEntryOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpiration;
        }

        if (options?.SlidingExpiration is not null)
        {
            distributedCacheEntryOptions.SlidingExpiration = options.SlidingExpiration;
        }

        return distributedCacheEntryOptions;
    }
}