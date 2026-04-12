namespace ZaminX.BuildingBlocks.Data.Abstractions.Models;

public sealed class PagedResult<TItems>
{
    public IReadOnlyCollection<TItems> Items { get; init; } = [];

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    public int ItemCount { get; init; }

    public long? TotalCount { get; init; }

    public int? TotalPages { get; init; }

    public bool HasPreviousPage { get; init; }

    public bool HasNextPage { get; init; }

}