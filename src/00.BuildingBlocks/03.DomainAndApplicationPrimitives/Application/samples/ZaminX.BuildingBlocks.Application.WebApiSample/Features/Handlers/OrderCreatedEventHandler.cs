using Relay.SampleWebApi.Features.Orders.Contracts;
using ZaminX.BuildingBlocks.Application.Events;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Features.Handlers;

public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "OrderCreatedEvent handled. OrderId: {OrderId}, CustomerName: {CustomerName}, Amount: {Amount}",
            @event.OrderId,
            @event.CustomerName,
            @event.Amount);

        return Task.CompletedTask;
    }
}