using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Builders;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Builders;

internal sealed class ParrotBuilder(IServiceCollection services) : IParrotBuilder
{
    public IServiceCollection Services { get; } = services;
}
