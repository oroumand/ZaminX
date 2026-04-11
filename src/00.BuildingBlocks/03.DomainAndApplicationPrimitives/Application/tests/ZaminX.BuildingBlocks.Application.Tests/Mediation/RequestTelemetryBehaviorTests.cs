using Microsoft.Extensions.Logging.Abstractions;
using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public class RequestTelemetryBehaviorTests
{
    private sealed class TestCommand : ICommand
    {
    }

    [Fact]
    public async Task Request_Telemetry_Behavior_Should_Return_Result_From_Next()
    {
        var logger = NullLogger<RequestTelemetryBehavior<TestCommand, Result>>.Instance;
        var behavior = new RequestTelemetryBehavior<TestCommand, Result>(logger);

        var result = await behavior.Handle(
            new TestCommand(),
            () => Task.FromResult(Result.Success()));

        Assert.True(result.IsSuccess);
    }
}