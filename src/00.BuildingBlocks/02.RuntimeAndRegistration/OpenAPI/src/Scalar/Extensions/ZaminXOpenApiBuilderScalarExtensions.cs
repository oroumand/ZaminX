using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar.Runtime;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar;

public static class ZaminXOpenApiBuilderScalarExtensions
{
    public static IZaminXOpenApiBuilder UseScalar(
        this IZaminXOpenApiBuilder builder,
        Action<ScalarUiOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var optionsBuilder = builder.Services.AddOptions<ScalarUiOptions>();

        if (builder.Configuration is not null)
        {
            optionsBuilder.Bind(
                builder.Configuration.GetSection($"{builder.SectionName}:{ScalarUiOptions.SectionName}"));
        }

        if (configure is not null)
        {
            builder.Services.PostConfigure(configure);
        }

        builder.Services.AddOptions<ScalarUiOptions>()
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.RoutePrefix),
                $"{nameof(ScalarUiOptions.RoutePrefix)} must not be null or empty.")
            .ValidateOnStart();

        builder.Services.AddSingleton<ILumenUiModule, ScalarUiModule>();

        return builder;
    }
}
