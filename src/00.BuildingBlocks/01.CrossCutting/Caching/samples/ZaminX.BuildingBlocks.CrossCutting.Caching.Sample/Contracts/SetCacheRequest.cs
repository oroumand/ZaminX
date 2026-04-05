namespace ZaminX.BuildingBlocks.CrossCutting.Caching.Sample.Contracts;

public sealed class SetCacheRequest
{
    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public int? AbsoluteExpirationSeconds { get; set; }

    public int? SlidingExpirationSeconds { get; set; }
}