using ZaminX.BuildingBlocks.Application.Configurations;
using ZaminX.BuildingBlocks.Application.Mediation;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXApplication(
        this IServiceCollection services,
        Action<RelayConfiguration>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var configuration = new RelayConfiguration();
        configure?.Invoke(configuration);

        services.AddScoped<IMediator, Mediator>();

        return services;
    }
}