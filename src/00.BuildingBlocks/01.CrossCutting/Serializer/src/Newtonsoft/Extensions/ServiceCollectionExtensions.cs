using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft.Configurations;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNewtonsoftJsonSerializer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new NewtonsoftJsonSerializerOptions();

        return services.AddNewtonsoftJsonSerializer(options);
    }

    public static IServiceCollection AddNewtonsoftJsonSerializer(
        this IServiceCollection services,
        Action<NewtonsoftJsonSerializerOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var options = new NewtonsoftJsonSerializerOptions();
        configure(options);

        return services.AddNewtonsoftJsonSerializer(options);
    }

    public static IServiceCollection AddNewtonsoftJsonSerializer(
        this IServiceCollection services,
        NewtonsoftJsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        var settings = options.ToJsonSerializerSettings();

        services.AddSingleton(settings);
        services.AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>();

        return services;
    }
}