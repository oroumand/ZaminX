using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc.Runtime;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc;

public static class ZaminXOpenApiBuilderRedocExtensions
{
    public static IZaminXOpenApiBuilder UseRedoc(
        this IZaminXOpenApiBuilder builder,
        Action<RedocUiOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var optionsBuilder = builder.Services.AddOptions<RedocUiOptions>();

        if (builder.Configuration is not null)
        {
            optionsBuilder.Bind(
                builder.Configuration.GetSection($"{builder.SectionName}:{RedocUiOptions.SectionName}"));
        }

        if (configure is not null)
        {
            builder.Services.PostConfigure(configure);
        }

        builder.Services.AddOptions<RedocUiOptions>()
            .Validate(
                options => options.RoutePrefix is not null,
                $"{nameof(RedocUiOptions.RoutePrefix)} must not be null.")
            .ValidateOnStart();

        builder.Services.AddSingleton<ILumenUiModule, RedocUiModule>();

        return builder;
    }
}
