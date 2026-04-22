using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeWriteRepository : IWriteRepository<FakeAggregateRoot, int>
{
    private readonly List<FakeAggregateRoot> _items = [];

    public Task<FakeAggregateRoot?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        return Task.FromResult<FakeAggregateRoot?>(item);
    }

    public Task AddAsync(FakeAggregateRoot aggregate, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        _items.Add(aggregate);
        return Task.CompletedTask;
    }

    public void Remove(FakeAggregateRoot aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        _items.Remove(aggregate);
    }

    public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        var exists = _items.Any(x => x.Id == id);
        return Task.FromResult(exists);
    }
}