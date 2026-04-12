using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreBuilderExtensions
{
    public static EntityFrameworkCoreBuilder<TDbContext> WithSqlServer<TDbContext>(
    this EntityFrameworkCoreBuilder<TDbContext> builder,
    string connectionString,
    Action<SqlServerDbContextOptionsBuilder>? configure = null)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException(
                "SQL Server connection string cannot be null or whitespace.",
                nameof(connectionString));
        }

        builder.SetProvider((_, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(connectionString, sqlServerOptionsBuilder =>
            {
                configure?.Invoke(sqlServerOptionsBuilder);
            });
        });

        return builder;
    }
}
