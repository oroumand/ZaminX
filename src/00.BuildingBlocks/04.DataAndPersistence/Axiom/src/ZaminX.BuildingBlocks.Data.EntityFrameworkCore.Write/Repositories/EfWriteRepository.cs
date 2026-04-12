using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Repositories;

public abstract class EfWriteRepository<TEntity, TId, TDbContext> :
IWriteRepository<TEntity, TId>
where TEntity : class
where TDbContext : DbContext
{
    protected TDbContext DbContext { get; }

    protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

    protected EfWriteRepository(TDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public virtual Task AddAsync(
        TEntity aggregate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        return Entities.AddAsync(aggregate, cancellationToken).AsTask();
    }

    public virtual void Remove(TEntity aggregate)
    {
        ArgumentNullException.ThrowIfNull(aggregate);

        Entities.Remove(aggregate);
    }

    public virtual Task<bool> ExistsAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        return CreateExistsQuery()
            .AnyAsync(BuildIdPredicate(id), cancellationToken);
    }

    public virtual Task<TEntity?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        return CreateAggregateQuery()
            .FirstOrDefaultAsync(BuildIdPredicate(id), cancellationToken);
    }

    protected virtual IQueryable<TEntity> CreateAggregateQuery()
    {
        return Entities;
    }

    protected virtual IQueryable<TEntity> CreateExistsQuery()
    {
        return Entities.AsNoTracking();
    }

    protected abstract Expression<Func<TEntity, bool>> BuildIdPredicate(TId id);


}
