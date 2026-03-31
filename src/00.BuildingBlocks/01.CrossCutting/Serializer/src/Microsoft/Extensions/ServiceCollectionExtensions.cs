using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft.Configurations;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMicrosoftJsonSerializer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new MicrosoftJsonSerializerOptions();

        return services.AddMicrosoftJsonSerializer(options);
    }

    public static IServiceCollection AddMicrosoftJsonSerializer(
        this IServiceCollection services,
        Action<MicrosoftJsonSerializerOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var options = new MicrosoftJsonSerializerOptions();
        configure(options);

        return services.AddMicrosoftJsonSerializer(options);
    }

    public static IServiceCollection AddMicrosoftJsonSerializer(
        this IServiceCollection services,
        MicrosoftJsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        var serializerOptions = options.ToJsonSerializerOptions();

        services.AddSingleton(serializerOptions);
        services.AddSingleton<IJsonSerializer, MicrosoftJsonSerializer>();

        return services;
    }
}