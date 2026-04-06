using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger.Runtime;

internal sealed class SwaggerUiModule(IOptions<SwaggerUiOptions> optionsAccessor) : ILumenUiModule
{
    public string Name => "Swagger";

    public void Map(WebApplication app, LumenRuntimeContext context)
    {
        var options = optionsAccessor.Value;
        if (!options.Enabled)
        {
            return;
        }

        app.UseSwaggerUI(swaggerUiOptions =>
        {
            swaggerUiOptions.RoutePrefix = Normalize(options.RoutePrefix);
            swaggerUiOptions.DocumentTitle = options.DocumentTitle;
            swaggerUiOptions.SwaggerEndpoint(context.ResolveDocumentUrl(), options.EndpointName);
        });
    }

    private static string Normalize(string routePrefix)
    {
        return routePrefix.Trim('/').Trim();
    }
}
