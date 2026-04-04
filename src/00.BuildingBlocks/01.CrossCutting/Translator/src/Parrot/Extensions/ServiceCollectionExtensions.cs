using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Builders;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Builders;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Exceptions;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddParrot(
        this IServiceCollection services,
        Action<IParrotBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.TryAddSingleton<ParrotTranslationStore>();
        services.TryAddSingleton<ParrotTranslationProviderCoordinator>();
        services.TryAddSingleton<ParrotRefreshService>();
        services.TryAddSingleton<ITranslationRefreshService>(sp => sp.GetRequiredService<ParrotRefreshService>());
        services.TryAddSingleton<ITranslationMissingKeyRegistrar, NullTranslationMissingKeyRegistrar>();
        services.TryAddSingleton<ITranslator, ParrotTranslator>();
        services.AddHostedService<ParrotStartupHostedService>();

        var builder = new ParrotBuilder(services);
        configure(builder);

        ValidateConfiguration(services);

        return services;
    }

    private static void ValidateConfiguration(IServiceCollection services)
    {
        var providerRegistrations = services.Count(descriptor =>
            descriptor.ServiceType == typeof(ITranslationDataProvider));

        if (providerRegistrations == 0)
        {
            throw new ParrotConfigurationException(
                "At least one translation data provider must be registered inside AddParrot(...).");
        }
    }
}