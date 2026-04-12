namespace ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}