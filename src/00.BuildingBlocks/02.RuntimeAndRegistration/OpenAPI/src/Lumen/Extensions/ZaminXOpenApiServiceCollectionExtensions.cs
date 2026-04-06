using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Configurations;

namespace Microsoft.Extensions.DependencyInjection;

public static class ZaminXOpenApiServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXOpenApi(
        this IServiceCollection services,
        Action<IZaminXOpenApiBuilder>? build = null)
    {
        return services.AddZaminXOpenApi(
            configuration: null,
            sectionName: LumenOptions.DefaultSectionName,
            configure: null,
            build: build);
    }

    public static IServiceCollection AddZaminXOpenApi(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = LumenOptions.DefaultSectionName,
        Action<IZaminXOpenApiBuilder>? build = null)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return services.AddZaminXOpenApi(
            configuration: configuration,
            sectionName: sectionName,
            configure: null,
            build: build);
    }

    public static IServiceCollection AddZaminXOpenApi(
        this IServiceCollection services,
        Action<LumenOptions> configure,
        Action<IZaminXOpenApiBuilder>? build = null)
    {
        ArgumentNullException.ThrowIfNull(configure);

        return services.AddZaminXOpenApi(
            configuration: null,
            sectionName: LumenOptions.DefaultSectionName,
            configure: configure,
            build: build);
    }

    private static IServiceCollection AddZaminXOpenApi(
        this IServiceCollection services,
        IConfiguration? configuration,
        string sectionName,
        Action<LumenOptions>? configure,
        Action<IZaminXOpenApiBuilder>? build)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        services.AddOpenApi();

        var optionsBuilder = services.AddOptions<LumenOptions>();

        if (configuration is not null)
        {
            optionsBuilder.Bind(configuration.GetSection(sectionName));
        }

        if (configure is not null)
        {
            services.PostConfigure(configure);
        }

        services.AddOptions<LumenOptions>()
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.DocumentPath),
                $"{nameof(LumenOptions.DocumentPath)} must not be null or empty.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.DefaultDocumentName),
                $"{nameof(LumenOptions.DefaultDocumentName)} must not be null or empty.")
            .ValidateOnStart();

        var builder = new ZaminXOpenApiBuilder(services, configuration, sectionName);
        build?.Invoke(builder);

        return services;
    }
}
