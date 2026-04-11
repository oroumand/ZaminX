using FluentValidation;
using Relay.SampleWebApi.Features.Orders.Contracts;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Features.Validators;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}