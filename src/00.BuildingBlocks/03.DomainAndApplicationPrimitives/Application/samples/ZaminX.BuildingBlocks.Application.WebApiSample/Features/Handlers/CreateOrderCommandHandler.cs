using Relay.SampleWebApi.Features.Orders.Contracts;
using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Features.Handlers;

public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly InMemoryOrderStore _store;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(InMemoryOrderStore store, IMediator mediator)
    {
        _store = store;
        _mediator = mediator;
    }

    public async Task<Result<Guid>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var orderId = _store.Add(command.CustomerName, command.Amount);

        await _mediator.Publish(
            new OrderCreatedEvent(orderId, command.CustomerName, command.Amount),
            cancellationToken);

        return Result<Guid>.Success(orderId);
    }
}
