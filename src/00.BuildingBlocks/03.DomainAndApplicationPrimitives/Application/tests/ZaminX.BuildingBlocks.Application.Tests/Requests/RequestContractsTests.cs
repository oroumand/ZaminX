using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Requests;

namespace ZaminX.BuildingBlocks.Application.Tests.Requests;

public class RequestContractsTests
{
    private sealed class TestRequest : IRequest<string>
    {
    }

    [Fact]
    public void Request_Should_Implement_Message()
    {
        var request = new TestRequest();

        Assert.IsAssignableFrom<IMessage>(request);
    }
}