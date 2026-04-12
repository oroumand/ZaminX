namespace ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

public interface IWriteRepository<TAggregate, TId>
{
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

    Task RemoveAsync(TAggregate aggregate);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

}