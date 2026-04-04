using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class ParrotBootstrapper(
    ParrotTranslationProviderCoordinator coordinator,
    ParrotTranslationStore store) : ITranslationRefreshService
{
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        var entries = await coordinator.LoadAllAsync(cancellationToken).ConfigureAwait(false);
        store.ReplaceAll(entries);
    }
}