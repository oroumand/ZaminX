namespace ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Infrastructure;

using Microsoft.Extensions.Hosting;
using ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Options;

internal sealed class SqlServerCachingStorageInitializerHostedService(
    SqlServerCachingOptions options,
    SqlServerCachingStorageInitializer initializer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (options.EnsureStorageOnStartup)
        {
            await initializer.InitializeAsync(options, cancellationToken);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}