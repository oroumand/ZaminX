using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;
using ZaminX.Samples.Data.SqlServer.Domain;

public interface IOrderWriteRepository : IWriteRepository<Order, long>
{
}