using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Commands;

public class CommandHandlerContractsTests
{
    private sealed class CreateOrderCommand : ICommand
    {
    }

    private sealed class CreateOrderWithIdCommand : ICommand<Guid>
    {
    }

    private sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        public Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result.Success());
        }
    }

    private sealed class CreateOrderWithIdCommandHandler : ICommandHandler<CreateOrderWithIdCommand, Guid>
    {
        public Task<Result<Guid>> Handle(CreateOrderWithIdCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<Guid>.Success(Guid.NewGuid()));
        }
    }

    [Fact]
    public async Task NonGeneric_CommandHandler_Should_Be_Implementable()
    {
        var handler = new CreateOrderCommandHandler();
        var command = new CreateOrderCommand();

        var result = await handler.Handle(command);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Generic_CommandHandler_Should_Be_Implementable()
    {
        var handler = new CreateOrderWithIdCommandHandler();
        var command = new CreateOrderWithIdCommand();

        var result = await handler.Handle(command);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
}