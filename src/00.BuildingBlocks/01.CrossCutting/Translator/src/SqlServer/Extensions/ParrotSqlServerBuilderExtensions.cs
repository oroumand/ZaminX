using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Builders;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ParrotSqlServerBuilderExtensions
{
    public static IParrotBuilder UseSqlServer(
         this IParrotBuilder builder,
         Action<ParrotSqlServerOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.Services.AddOptions<ParrotSqlServerOptions>().Configure(configure);

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ITranslationDataProvider, SqlServerTranslationDataProvider>());

        builder.Services.AddHostedService<SqlServerTranslationReloadHostedService>();

        builder.Services.Replace(
            ServiceDescriptor.Singleton<ITranslationMissingKeyRegistrar, SqlServerTranslationMissingKeyRegistrar>());

        return builder;
    }
}