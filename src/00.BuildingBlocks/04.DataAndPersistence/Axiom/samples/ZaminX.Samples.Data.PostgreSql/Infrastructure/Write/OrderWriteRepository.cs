using System.Linq.Expressions;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Repositories;
using ZaminX.Samples.Data.PostgreSql.Contracts;
using ZaminX.Samples.Data.PostgreSql.Domain;

namespace ZaminX.Samples.Data.PostgreSql.Infrastructure.Write;

public sealed class OrderWriteRepository
    : EfWriteRepository<Order, long, AppWriteDbContext>, IOrderWriteRepository
{
    public OrderWriteRepository(AppWriteDbContext dbContext)
        : base(dbContext)
    {
    }

    protected override Expression<Func<Order, bool>> BuildIdPredicate(long id)
    {
        return order => order.Id == id;
    }
}