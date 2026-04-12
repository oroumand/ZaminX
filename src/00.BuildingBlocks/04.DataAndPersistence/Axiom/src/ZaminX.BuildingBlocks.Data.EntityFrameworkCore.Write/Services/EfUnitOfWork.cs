using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Services;

public class EfUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork
where TDbContext : DbContext
{
    protected TDbContext DbContext { get; } = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }

}
