namespace Relay.SampleWebApi.Features.Orders.Contracts;

public sealed record OrderDto(Guid Id, string CustomerName, decimal Amount);
