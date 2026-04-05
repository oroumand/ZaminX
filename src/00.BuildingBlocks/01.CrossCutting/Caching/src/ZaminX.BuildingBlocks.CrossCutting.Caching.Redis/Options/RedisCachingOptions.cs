namespace ZaminX.BuildingBlocks.CrossCutting.Caching.Redis.Options;

public sealed class RedisCachingOptions
{
    public string Configuration { get; set; } = string.Empty;

    public string? InstanceName { get; set; }
}