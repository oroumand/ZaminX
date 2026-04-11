using Microsoft.Extensions.Logging.Abstractions;
using ZaminX.BuildingBlocks.Application.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public class ExceptionToResultBehaviorTests
{
    private sealed class TestMessage : IMessage
    {
    }

    private sealed class CodedException : Exception
    {
        public string Code { get; }

        public CodedException(string code, string message) : base(message)
        {
            Code = code;
        }
    }

    [Fact]
    public async Task Exception_Behavior_Should_Use_Exception_Code_When_Available()
    {
        var logger = NullLogger<ExceptionToResultBehavior<TestMessage, Result>>.Instance;
        var behavior = new ExceptionToResultBehavior<TestMessage, Result>(logger);

        var result = await behavior.Handle(
            new TestMessage(),
            () => throw new CodedException("orders.invalid-state", "Order is invalid."));

        Assert.True(result.IsFailure);
        Assert.Equal("orders.invalid-state", result.FirstError!.Code);
    }

    [Fact]
    public async Task Exception_Behavior_Should_Use_Default_Code_When_Exception_Has_No_Code()
    {
        var logger = NullLogger<ExceptionToResultBehavior<TestMessage, Result>>.Instance;
        var behavior = new ExceptionToResultBehavior<TestMessage, Result>(logger);

        var result = await behavior.Handle(
            new TestMessage(),
            () => throw new InvalidOperationException("Unexpected."));

        Assert.True(result.IsFailure);
        Assert.Equal("application.unhandled-exception", result.FirstError!.Code);
    }
}