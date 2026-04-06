using Microsoft.AspNetCore.Builder;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Models;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Contracts;

public interface ILumenUiModule
{
    string Name { get; }

    void Map(WebApplication app, LumenRuntimeContext context);
}
