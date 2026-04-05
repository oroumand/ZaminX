namespace ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Models;

public sealed class CacheEntryOptions
{
    public TimeSpan? AbsoluteExpiration { get; set; }

    public TimeSpan? SlidingExpiration { get; set; }
}