using Relay.SampleWebApi.Features.Orders.Contracts;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

public sealed class InMemoryOrderStore
{
    private readonly Dictionary<Guid, OrderDto> _orders = new();

    public Guid Add(string customerName, decimal amount)
    {
        var id = Guid.NewGuid();
        _orders[id] = new OrderDto(id, customerName, amount);
        return id;
    }

    public OrderDto? GetById(Guid id)
    {
        return _orders.TryGetValue(id, out var order) ? order : null;
    }
}