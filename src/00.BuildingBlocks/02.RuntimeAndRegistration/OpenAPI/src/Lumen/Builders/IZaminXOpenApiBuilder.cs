using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;

public interface IZaminXOpenApiBuilder
{
    IServiceCollection Services { get; }

    IConfiguration? Configuration { get; }

    string SectionName { get; }
}
