using System.Globalization;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

public interface ITranslationMissingKeyRegistrar
{
    Task RegisterIfNeededAsync(string key, CultureInfo? culture, CancellationToken cancellationToken = default);
}