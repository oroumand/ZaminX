using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreBuilderExtensions
{
    public static EntityFrameworkCoreBuilder<TDbContext> WithPostgreSql<TDbContext>(
    this EntityFrameworkCoreBuilder<TDbContext> builder,
    string connectionString,
    Action<NpgsqlDbContextOptionsBuilder>? configure = null)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);


        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "PostgreSQL connection string cannot be null or whitespace.",
                nameof(connectionString));
        }

        builder.SetProvider((_, optionsBuilder) =>
        {
            optionsBuilder.UseNpgsql(connectionString, npgsqlOptionsBuilder =>
            {
                configure?.Invoke(npgsqlOptionsBuilder);
            });
        });

        return builder;
    }


}
