using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;
/// <summary>
/// سرویس درونی Parrot برای اجرای refresh ترجمه‌ها.
/// این کلاس hosted service نیست و توسط hosted serviceهای startup یا provider-specific فراخوانی می‌شود.
/// </summary>
internal sealed class ParrotRefreshService(
    ParrotTranslationProviderCoordinator coordinator,
    ParrotTranslationStore store) : ITranslationRefreshService
{
    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        var entries = await coordinator.LoadAllAsync(cancellationToken).ConfigureAwait(false);
        store.ReplaceAll(entries);
    }
}