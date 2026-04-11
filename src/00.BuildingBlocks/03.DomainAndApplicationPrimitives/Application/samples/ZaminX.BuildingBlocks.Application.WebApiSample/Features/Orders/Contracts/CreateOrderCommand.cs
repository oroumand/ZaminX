using ZaminX.BuildingBlocks.Application.Commands;

namespace Relay.SampleWebApi.Features.Orders.Contracts;

public sealed record CreateOrderCommand(string CustomerName, decimal Amount) : ICommand<Guid>;