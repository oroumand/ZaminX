using System.Linq.Expressions;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.Samples.Data.PostgreSql.Contracts;
using ZaminX.Samples.Data.PostgreSql.Domain;

namespace ZaminX.Samples.Data.PostgreSql.Infrastructure.Read;

public sealed class OrderReadRepository
    : EfReadRepository<Order, long, AppReadDbContext>, IOrderReadRepository
{
    public OrderReadRepository(AppReadDbContext dbContext)
        : base(dbContext)
    {
    }

    protected override Expression<Func<Order, bool>> BuildIdPredicate(long id)
    {
        return order => order.Id == id;
    }

    protected override IQueryable<Order> CreatePagedQuery(PagedQuery request)
    {
        return base.CreatePagedQuery(request);
    }
}