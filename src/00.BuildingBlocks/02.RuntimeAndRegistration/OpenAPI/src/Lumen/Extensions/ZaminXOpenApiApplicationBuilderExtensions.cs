using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Configurations;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;

namespace Microsoft.AspNetCore.Builder;

public static class ZaminXOpenApiApplicationBuilderExtensions
{
    public static WebApplication UseZaminXOpenApi(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var options = app.Services.GetRequiredService<IOptions<LumenOptions>>().Value;
        if (!options.Enabled)
        {
            return app;
        }

        app.MapOpenApi(options.DocumentPath);

        var context = new LumenRuntimeContext(
            options.DefaultDocumentName,
            options.DocumentPath);

        var modules = app.Services.GetServices<ILumenUiModule>();
        foreach (var module in modules)
        {
            module.Map(app, context);
        }

        return app;
    }
}
