using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;
using ZaminX.Samples.Data.PostgreSql.Domain;

public interface IOrderWriteRepository : IWriteRepository<Order, long>
{
}