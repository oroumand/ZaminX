using FluentValidation;
using Relay.SampleWebApi.Features.Orders.Contracts;
using ZaminX.BuildingBlocks.Application.Validation;
using ZaminX.BuildingBlocks.Application.WebApiSample;
using ZaminX.BuildingBlocks.Application.WebApiSample.Behaviors;
using ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddRelaySample(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddHttpContextAccessor();

        services.AddSingleton<InMemoryOrderStore>();

        services.AddZaminXApplication(options =>
        {
            options.EnableRequestTelemetryBehavior = true;
            options.EnableValidationBehavior = true;
            options.EnableExceptionToResultBehavior = true;

            options.AddOpenBehavior(typeof(HeaderLoggingBehavior<,>));
        });

        services.AddZaminXApplicationHandlers(typeof(SampleAssemblyMarker).Assembly);

        services.AddValidatorsFromAssemblyContaining<SampleAssemblyMarker>();

        services.AddScoped<IMessageValidator<CreateOrderCommand>, FluentValidationMessageValidatorAdapter<CreateOrderCommand>>();

        return services;
    }
}