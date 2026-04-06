using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Builders;

internal sealed class ZaminXOpenApiBuilder(
    IServiceCollection services,
    IConfiguration? configuration,
    string sectionName) : IZaminXOpenApiBuilder
{
    public IServiceCollection Services { get; } = services;

    public IConfiguration? Configuration { get; } = configuration;

    public string SectionName { get; } = sectionName;
}
