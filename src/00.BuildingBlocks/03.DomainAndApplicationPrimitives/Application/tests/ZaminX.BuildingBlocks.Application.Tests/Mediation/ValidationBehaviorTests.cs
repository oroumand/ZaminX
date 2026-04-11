using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.Validation;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public class ValidationBehaviorTests
{
    private sealed class CreateOrderCommand : ICommand
    {
    }

    private sealed class CreateOrderValidator : IMessageValidator<CreateOrderCommand>
    {
        public Task<IReadOnlyCollection<Error>> ValidateAsync(
            CreateOrderCommand message,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<Error> errors =
            [
                new Error("orders.invalid", "Order is invalid.")
            ];

            return Task.FromResult(errors);
        }
    }

    [Fact]
    public async Task Validation_Behavior_Should_Return_Failure_When_Errors_Exist()
    {
        var behavior = new ValidationBehavior<CreateOrderCommand, Result>(
            new IMessageValidator<CreateOrderCommand>[] { new CreateOrderValidator() });

        var result = await behavior.Handle(
            new CreateOrderCommand(),
            () => Task.FromResult(Result.Success()));

        Assert.True(result.IsFailure);
        Assert.Single(result.Errors);
        Assert.Equal("orders.invalid", result.FirstError!.Code);
    }
}