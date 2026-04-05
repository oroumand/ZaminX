namespace Microsoft.Extensions.DependencyInjection;

using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Infrastructure;
using ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Options;
using ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXCachingWithSqlServer(
        this IServiceCollection services,
        Action<SqlServerCachingOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var sqlServerCachingOptions = new SqlServerCachingOptions();
        configure(sqlServerCachingOptions);

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = sqlServerCachingOptions.ConnectionString;
            options.SchemaName = sqlServerCachingOptions.SchemaName;
            options.TableName = sqlServerCachingOptions.TableName;
        });

        services.AddSingleton(sqlServerCachingOptions);

        if (sqlServerCachingOptions.EnsureStorageOnStartup)
        {
            services.AddSingleton<SqlServerCachingStorageInitializer>();
            services.AddHostedService<SqlServerCachingStorageInitializerHostedService>();
        }

        services.AddSingleton<IStashX, SqlServerStashX>();

        return services;
    }
}