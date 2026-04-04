using System.Globalization;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class NullTranslationMissingKeyRegistrar : ITranslationMissingKeyRegistrar
{
    public Task RegisterIfNeededAsync(string key, CultureInfo? culture, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}