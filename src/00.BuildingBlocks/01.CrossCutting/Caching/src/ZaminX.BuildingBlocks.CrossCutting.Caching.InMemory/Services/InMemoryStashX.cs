namespace ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory.Services;

using Microsoft.Extensions.Caching.Memory;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Models;

public sealed class InMemoryStashX(IMemoryCache memoryCache) : IStashX
{
    public Task SetAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var memoryCacheEntryOptions = CreateMemoryCacheEntryOptions(options);

        memoryCache.Set(key, value, memoryCacheEntryOptions);

        return Task.CompletedTask;
    }

    public Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        memoryCache.TryGetValue(key, out T? value);

        return Task.FromResult(value);
    }

    public async Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(key, out T? cachedValue) && cachedValue is not null)
        {
            return cachedValue;
        }

        var value = await factory(cancellationToken);

        var memoryCacheEntryOptions = CreateMemoryCacheEntryOptions(options);

        memoryCache.Set(key, value, memoryCacheEntryOptions);

        return value;
    }

    public Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        memoryCache.Remove(key);

        return Task.CompletedTask;
    }

    private static MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheEntryOptions? options)
    {
        var memoryCacheEntryOptions = new MemoryCacheEntryOptions();

        if (options?.AbsoluteExpiration is not null)
        {
            memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = options.AbsoluteExpiration;
        }

        if (options?.SlidingExpiration is not null)
        {
            memoryCacheEntryOptions.SlidingExpiration = options.SlidingExpiration;
        }

        return memoryCacheEntryOptions;
    }
}