using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger.Runtime;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger;

public static class ZaminXOpenApiBuilderSwaggerExtensions
{
    public static IZaminXOpenApiBuilder UseSwagger(
        this IZaminXOpenApiBuilder builder,
        Action<SwaggerUiOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var optionsBuilder = builder.Services.AddOptions<SwaggerUiOptions>();

        if (builder.Configuration is not null)
        {
            optionsBuilder.Bind(
                builder.Configuration.GetSection($"{builder.SectionName}:{SwaggerUiOptions.SectionName}"));
        }

        if (configure is not null)
        {
            builder.Services.PostConfigure(configure);
        }

        builder.Services.AddOptions<SwaggerUiOptions>()
            .Validate(
                options => options.RoutePrefix is not null,
                $"{nameof(SwaggerUiOptions.RoutePrefix)} must not be null.")
            .ValidateOnStart();

        builder.Services.AddSingleton<ILumenUiModule, SwaggerUiModule>();

        return builder;
    }
}
