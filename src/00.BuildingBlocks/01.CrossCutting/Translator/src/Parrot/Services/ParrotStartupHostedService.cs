using Microsoft.Extensions.Hosting;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot.Services;

internal sealed class ParrotStartupHostedService(
    ITranslationRefreshService refreshService) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await refreshService.RefreshAsync(cancellationToken).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}