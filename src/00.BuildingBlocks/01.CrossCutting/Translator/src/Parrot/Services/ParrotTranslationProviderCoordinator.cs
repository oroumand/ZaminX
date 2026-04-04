using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Models;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class ParrotTranslationProviderCoordinator(IEnumerable<ITranslationDataProvider> providers)
{
    private readonly IReadOnlyList<ITranslationDataProvider> _providers = [.. providers];

    public async Task<IReadOnlyCollection<TranslationEntry>> LoadAllAsync(CancellationToken cancellationToken = default)
    {
        var mergedEntries = new List<TranslationEntry>();

        foreach (var provider in _providers)
        {
            var entries = await provider.LoadAsync(cancellationToken).ConfigureAwait(false);
            mergedEntries.AddRange(entries);
        }

        return mergedEntries;
    }
}