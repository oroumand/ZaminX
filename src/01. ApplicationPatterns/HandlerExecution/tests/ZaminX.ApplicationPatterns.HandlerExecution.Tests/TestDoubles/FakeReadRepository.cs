using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeReadRepository : IReadRepository<FakeEntity, int>
{
    private readonly List<FakeEntity> _items = [];

    public Task<FakeEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        return Task.FromResult<FakeEntity?>(item);
    }

    public Task<PagedResult<FakeEntity>> GetPagedListAsync(
        PagedQuery request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var totalCount = _items.Count;
        var items = _items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<FakeEntity>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return Task.FromResult(result);
    }

    public FakeReadRepository Seed(params FakeEntity[] entities)
    {
        _items.AddRange(entities);
        return this;
    }
}