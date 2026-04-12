using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;
using ZaminX.Samples.Data.SqlServer.Contracts;
using ZaminX.Samples.Data.SqlServer.Domain;

namespace ZaminX.Samples.Data.SqlServer.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/orders");

        group.MapPost("/", CreateOrderAsync);
        group.MapGet("/", GetPagedOrdersAsync);
        group.MapGet("/{id:long}", GetOrderByIdAsync);

        return endpoints;
    }

    private static async Task<IResult> CreateOrderAsync(
        CreateOrderRequest request,
        IOrderWriteRepository repository,
        IUnitOfWork unitOfWork,
        CancellationToken cancellationToken)
    {
        var order = new Order
        {
            CustomerName = request.CustomerName,
            Amount = request.Amount,
            Description = request.Description
        };

        await repository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Results.Created($"/orders/{order.Id}", order);
    }

    private static async Task<IResult> GetPagedOrdersAsync(
        int pageNumber,
        int pageSize,
        bool includeTotalCount,
        string? sortBy,
        bool sortDescending,
        IOrderReadRepository repository,
        CancellationToken cancellationToken)
    {
        var request = new PagedQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            IncludeTotalCount = includeTotalCount,
            SortBy = sortBy,
            SortDescending = sortDescending
        };

        var result = await repository.GetPagedListAsync(request, cancellationToken);

        return Results.Ok(result);
    }

    private static async Task<IResult> GetOrderByIdAsync(
        long id,
        IOrderReadRepository repository,
        CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(id, cancellationToken);

        return order is null ? Results.NotFound() : Results.Ok(order);
    }

    public sealed class CreateOrderRequest
    {
        public string CustomerName { get; init; } = string.Empty;

        public decimal Amount { get; init; }

        public string Description { get; init; } = string.Empty;
    }
}