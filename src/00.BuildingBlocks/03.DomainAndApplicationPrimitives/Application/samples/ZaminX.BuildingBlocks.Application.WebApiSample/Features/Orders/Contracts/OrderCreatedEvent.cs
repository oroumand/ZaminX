using ZaminX.BuildingBlocks.Application.Events;

namespace Relay.SampleWebApi.Features.Orders.Contracts;

public sealed record OrderCreatedEvent(Guid OrderId, string CustomerName, decimal Amount) : IEvent;