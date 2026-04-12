using System.Linq.Expressions;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Repositories;
using ZaminX.Samples.Data.SqlServer.Contracts;
using ZaminX.Samples.Data.SqlServer.Domain;

namespace ZaminX.Samples.Data.SqlServer.Infrastructure.Write;

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