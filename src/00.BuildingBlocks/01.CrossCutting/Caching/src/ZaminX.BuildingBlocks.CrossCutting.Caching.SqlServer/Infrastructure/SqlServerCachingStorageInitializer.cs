namespace ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Infrastructure;

using Microsoft.Data.SqlClient;
using ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer.Options;

internal sealed class SqlServerCachingStorageInitializer
{
    public async Task InitializeAsync(
        SqlServerCachingOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        var commandText = $"""
IF NOT EXISTS (
    SELECT 1
    FROM sys.schemas
    WHERE name = N'{EscapeSqlLiteral(options.SchemaName)}'
)
BEGIN
    EXEC(N'CREATE SCHEMA [{EscapeSqlIdentifier(options.SchemaName)}]')
END;

IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_SCHEMA = N'{EscapeSqlLiteral(options.SchemaName)}'
      AND TABLE_NAME = N'{EscapeSqlLiteral(options.TableName)}'
)
BEGIN
    CREATE TABLE [{EscapeSqlIdentifier(options.SchemaName)}].[{EscapeSqlIdentifier(options.TableName)}]
    (
        [Id] nvarchar(449) NOT NULL PRIMARY KEY,
        [Value] varbinary(max) NOT NULL,
        [ExpiresAtTime] datetimeoffset NOT NULL,
        [SlidingExpirationInSeconds] bigint NULL,
        [AbsoluteExpiration] datetimeoffset NULL
    );
END;
""";

        await using var connection = new SqlConnection(options.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = commandText;

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static string EscapeSqlLiteral(string value)
    {
        return value.Replace("'", "''");
    }

    private static string EscapeSqlIdentifier(string value)
    {
        return value.Replace("]", "]]");
    }
}