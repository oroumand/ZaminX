using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar.Runtime;

internal sealed class ScalarUiModule(IOptions<ScalarUiOptions> optionsAccessor) : ILumenUiModule
{
    public string Name => "Scalar";

    public void Map(WebApplication app, LumenRuntimeContext context)
    {
        var options = optionsAccessor.Value;
        if (!options.Enabled)
        {
            return;
        }

        app.MapScalarApiReference(options.RoutePrefix, scalarOptions =>
        {
            scalarOptions.WithOpenApiRoutePattern(context.DocumentPathPattern);

            if (!string.IsNullOrWhiteSpace(options.Title))
            {
                scalarOptions.WithTitle(options.Title);
            }
        });
    }
}
