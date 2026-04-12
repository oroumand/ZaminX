namespace ZaminX.BuildingBlocks.Data.Abstractions.Models;

public sealed class PagedQuery<TSearch> : PagedQuery
{
    public TSearch? Search { get; init; }
}