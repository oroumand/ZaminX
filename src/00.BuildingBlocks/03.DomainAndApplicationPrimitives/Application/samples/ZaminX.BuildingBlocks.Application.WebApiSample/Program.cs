using Relay.SampleWebApi.Features.Orders.Contracts;
using Relay.SampleWebApi.Infrastructure;
using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddRelaySample();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();


app.MapPost("/orders", async (
    CreateOrderCommand command,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(command, cancellationToken);
    return result.ToHttpResult();
});

app.MapGet("/orders/{id:guid}", async (
    Guid id,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
    return result.ToHttpResult();
});

app.MapPost("/orders/{id:guid}/events", async (
    Guid id,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Publish(new OrderCreatedEvent(id, "Manual Event", 0), cancellationToken);
    return Results.Ok(new
    {
        success = true,
        message = "Event published."
    });
});

app.Run();