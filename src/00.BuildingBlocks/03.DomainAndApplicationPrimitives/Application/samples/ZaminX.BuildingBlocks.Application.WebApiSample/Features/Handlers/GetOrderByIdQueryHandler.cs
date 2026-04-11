using Relay.SampleWebApi.Features.Orders.Contracts;
using ZaminX.BuildingBlocks.Application.Queries;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Features.Handlers;

public sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly InMemoryOrderStore _store;

    public GetOrderByIdQueryHandler(InMemoryOrderStore store)
    {
        _store = store;
    }

    public Task<Result<OrderDto>> Handle(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var order = _store.GetById(query.Id);

        if (order is null)
        {
            return Task.FromResult(
                Result<OrderDto>.Failure(
                    new Error("orders.not-found", $"Order '{query.Id}' was not found.")));
        }

        return Task.FromResult(Result<OrderDto>.Success(order));
    }
}
