using Microsoft.Extensions.DependencyInjection;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Builders;

public interface IParrotBuilder
{
    IServiceCollection Services { get; }
}
