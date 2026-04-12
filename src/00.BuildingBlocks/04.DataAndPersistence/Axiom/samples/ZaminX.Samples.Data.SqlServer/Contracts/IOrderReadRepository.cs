using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;
using ZaminX.Samples.Data.SqlServer.Domain;

namespace ZaminX.Samples.Data.SqlServer.Contracts;

public interface IOrderReadRepository : IReadRepository<Order, long>
{
}