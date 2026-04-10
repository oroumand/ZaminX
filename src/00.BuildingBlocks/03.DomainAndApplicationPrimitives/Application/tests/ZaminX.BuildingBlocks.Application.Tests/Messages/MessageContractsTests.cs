using ZaminX.BuildingBlocks.Application.Messages;

namespace ZaminX.BuildingBlocks.Application.Tests.Messages;

public class MessageContractsTests
{
    private sealed class TestMessage : IMessage
    {
    }

    [Fact]
    public void Message_Should_Be_Implementable()
    {
        var message = new TestMessage();

        Assert.NotNull(message);
        Assert.IsType<IMessage>(message, exactMatch: false);
    }
}