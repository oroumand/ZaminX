using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Extensions;
using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;

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

        // 👇 فقط وقتی SortBy نداشت
        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = ApplyDefaultSorting(query);
        }

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

    /// <summary>
    /// Default sorting hook (fallback when no SortBy is provided)
    /// </summary>
    protected virtual IQueryable<TEntity> ApplyDefaultSorting(IQueryable<TEntity> query)
    {
        return TryApplyIdDescending(query);
    }

    protected abstract Expression<Func<TEntity, bool>> BuildIdPredicate(TId id);

    // 👇 internal helper
    private static IQueryable<TEntity> TryApplyIdDescending(IQueryable<TEntity> query)
    {
        var property = typeof(TEntity)
            .GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);

        if (property is null)
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var propertyAccess = Expression.Property(parameter, property);
        var lambda = Expression.Lambda(propertyAccess, parameter);

        var methodCall = Expression.Call(
            typeof(Queryable),
            "OrderByDescending",
            [typeof(TEntity), property.PropertyType],
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<TEntity>(methodCall);
    }


}
