using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Requests;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Tests.Commands;

public class CommandContractsTests
{
    private sealed class NonGenericCommand : ICommand
    {
    }

    private sealed class GenericCommand : ICommand<Guid>
    {
    }

    [Fact]
    public void NonGeneric_Command_Should_Implement_Message()
    {
        var command = new NonGenericCommand();

        Assert.IsType<IMessage>(command, exactMatch: false);
    }

    [Fact]
    public void NonGeneric_Command_Should_Implement_Request_Of_Result()
    {
        var command = new NonGenericCommand();

        Assert.IsType<IRequest<Result>>(command, exactMatch: false);
    }

    [Fact]
    public void Generic_Command_Should_Implement_Message()
    {
        var command = new GenericCommand();

        Assert.IsType<IMessage>(command, exactMatch: false);
    }

    [Fact]
    public void Generic_Command_Should_Implement_Request_Of_ResultOfT()
    {
        var command = new GenericCommand();

        Assert.IsType<IRequest<Result<Guid>>>(command, exactMatch: false);
    }
}