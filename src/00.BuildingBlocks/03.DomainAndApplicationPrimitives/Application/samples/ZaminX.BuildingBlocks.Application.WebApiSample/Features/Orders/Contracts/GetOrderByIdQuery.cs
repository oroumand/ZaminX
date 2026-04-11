using ZaminX.BuildingBlocks.Application.Queries;

namespace Relay.SampleWebApi.Features.Orders.Contracts;

public sealed record GetOrderByIdQuery(Guid Id) : IQuery<OrderDto>;