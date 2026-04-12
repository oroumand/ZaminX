using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Extensions;
using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Repositories;

public abstract class EfReadRepository<TEntity, TId, TDbContext>(TDbContext dbContext) :
IReadRepository<TEntity, TId>
where TEntity : class
where TDbContext : DbContext
{
    protected TDbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    protected DbSet<TEntity> Entities => DbContext.Set<TEntity>();

    public virtual Task<PagedResult<TEntity>> GetPagedListAsync(
        PagedQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = CreatePagedQuery(request);

        return query.ToPagedResultAsync(request, cancellationToken);
    }

    public virtual Task<TEntity?> GetByIdAsync(
        TId id,
        CancellationToken cancellationToken = default)
    {
        return CreateSingleQuery()
            .FirstOrDefaultAsync(BuildIdPredicate(id), cancellationToken);
    }

    protected virtual IQueryable<TEntity> CreateSingleQuery()
    {
        return Entities.AsNoTracking();
    }

    protected virtual IQueryable<TEntity> CreatePagedQuery(PagedQuery request)
    {
        return Entities.AsNoTracking();
    }

    protected abstract Expression<Func<TEntity, bool>> BuildIdPredicate(TId id);

}
