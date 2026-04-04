using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Services;

internal sealed class SqlServerTranslationReloadHostedService(
    IOptions<ParrotSqlServerOptions> options,
    ITranslationRefreshService refreshService) : BackgroundService
{
    private readonly ParrotSqlServerOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.ReloadMode != TranslationReloadMode.Periodic)
        {
            return;
        }

        if (!_options.ReloadInterval.HasValue || _options.ReloadInterval.Value <= TimeSpan.Zero)
        {
            return;
        }

        using var timer = new PeriodicTimer(_options.ReloadInterval.Value);

        while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false))
        {
            await refreshService.RefreshAsync(stoppingToken).ConfigureAwait(false);
        }
    }
}