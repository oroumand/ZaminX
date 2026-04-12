using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;
using ZaminX.Samples.Data.PostgreSql.Domain;

namespace ZaminX.Samples.Data.PostgreSql.Contracts;

public interface IOrderReadRepository : IReadRepository<Order, long>
{
}