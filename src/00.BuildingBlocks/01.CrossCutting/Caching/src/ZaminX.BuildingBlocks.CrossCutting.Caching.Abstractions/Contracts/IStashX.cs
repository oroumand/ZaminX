namespace ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;

using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Models;

public interface IStashX
{
    Task SetAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default);

    Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default);
}