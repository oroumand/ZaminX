using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Queries;
using ZaminX.BuildingBlocks.Application.Requests;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Tests.Queries;

public class QueryContractsTests
{
    private sealed class TestQuery : IQuery<Guid>
    {
    }

    [Fact]
    public void Query_Should_Implement_Message()
    {
        var query = new TestQuery();

        Assert.IsType<IMessage>(query, exactMatch: false);
    }

    [Fact]
    public void Query_Should_Implement_Request_Of_ResultOfT()
    {
        var query = new TestQuery();

        Assert.IsType<IRequest<Result<Guid>>>(query, exactMatch: false);
    }
}