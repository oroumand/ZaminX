using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.ReDoc;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc.Configurations;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc.Runtime;

internal sealed class RedocUiModule(IOptions<RedocUiOptions> optionsAccessor) : ILumenUiModule
{
    public string Name => "Redoc";

    public void Map(WebApplication app, LumenRuntimeContext context)
    {
        var options = optionsAccessor.Value;
        if (!options.Enabled)
        {
            return;
        }

        app.UseReDoc(redocOptions =>
        {
            redocOptions.RoutePrefix = Normalize(options.RoutePrefix);
            redocOptions.DocumentTitle = options.DocumentTitle;
            redocOptions.SpecUrl = context.ResolveDocumentUrl();
        });
    }

    private static string Normalize(string routePrefix)
    {
        return routePrefix.Trim('/').Trim();
    }
}
