using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Internals;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Extensions;

public static class QueryablePagingExtensions
{
    public static async Task<PagedResult<TEntity>> ToPagedResultAsync<TEntity>(
    this IQueryable<TEntity> query,
    PagedQuery request,
    CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(request);

    var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 20 : request.PageSize;

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = QueryableSortingHelper.ApplySorting(
                query,
                request.SortBy,
                request.SortDescending);
        }

        if (request.IncludeTotalCount)
        {
            var totalCount = await query.LongCountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var totalPages = totalCount == 0
                ? 0
                : (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<TEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                ItemCount = items.Count,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };
        }

        var pageItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync(cancellationToken);

        var hasNextPage = pageItems.Count > pageSize;

        if (hasNextPage)
        {
            pageItems.RemoveAt(pageItems.Count - 1);
        }

        return new PagedResult<TEntity>
        {
            Items = pageItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            ItemCount = pageItems.Count,
            TotalCount = null,
            TotalPages = null,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = hasNextPage
        };
    }

}
