using FluentValidation;
using Relay.SampleWebApi.Features.Orders.Contracts;
using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Events;
using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.Mediation.Behaviors;
using ZaminX.BuildingBlocks.Application.Queries;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.Validation;
using ZaminX.BuildingBlocks.Application.WebApiSample.Behaviors;
using ZaminX.BuildingBlocks.Application.WebApiSample.Features.Handlers;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRelaySample(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddSingleton<InMemoryOrderStore>();

        services.AddScoped<IMediator, Mediator>();

        services.AddScoped<ICommandHandler<CreateOrderCommand, Guid>, CreateOrderCommandHandler>();
        services.AddScoped<IQueryHandler<GetOrderByIdQuery, OrderDto>, GetOrderByIdQueryHandler>();
        services.AddScoped<IEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();

        services.AddValidatorsFromAssemblyContaining<Program>();

        services.AddScoped<IMessageValidator<CreateOrderCommand>, FluentValidationMessageValidatorAdapter<CreateOrderCommand>>();

        services.AddScoped<IMessageBehavior<CreateOrderCommand, Result<Guid>>, RequestTelemetryBehavior<CreateOrderCommand, Result<Guid>>>();
        services.AddScoped<IMessageBehavior<CreateOrderCommand, Result<Guid>>, ValidationBehavior<CreateOrderCommand, Result<Guid>>>();
        services.AddScoped<IMessageBehavior<CreateOrderCommand, Result<Guid>>, HeaderLoggingBehavior<CreateOrderCommand, Result<Guid>>>();
        services.AddScoped<IMessageBehavior<CreateOrderCommand, Result<Guid>>, ExceptionToResultBehavior<CreateOrderCommand, Result<Guid>>>();

        services.AddScoped<IMessageBehavior<GetOrderByIdQuery, Result<OrderDto>>, RequestTelemetryBehavior<GetOrderByIdQuery, Result<OrderDto>>>();
        services.AddScoped<IMessageBehavior<GetOrderByIdQuery, Result<OrderDto>>, ValidationBehavior<GetOrderByIdQuery, Result<OrderDto>>>();
        services.AddScoped<IMessageBehavior<GetOrderByIdQuery, Result<OrderDto>>, HeaderLoggingBehavior<GetOrderByIdQuery, Result<OrderDto>>>();
        services.AddScoped<IMessageBehavior<GetOrderByIdQuery, Result<OrderDto>>, ExceptionToResultBehavior<GetOrderByIdQuery, Result<OrderDto>>>();

        return services;
    }
}