using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Models;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Services;

internal sealed class SqlServerTranslationDataProvider(
    IOptions<ParrotSqlServerOptions> options) : ITranslationDataProvider
{
    private readonly ParrotSqlServerOptions _options = options.Value;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private volatile bool _initialized;

    public async Task<IReadOnlyCollection<TranslationEntry>> LoadAsync(CancellationToken cancellationToken = default)
    {
        ValidateOptions();

        await EnsureInitializedAsync(cancellationToken).ConfigureAwait(false);

        var entries = new List<TranslationEntry>();

        await using var connection = new SqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"SELECT [Key], [Culture], [Value] FROM {GetQualifiedTableName()} ORDER BY [Id] ASC;";
        ApplyCommandTimeout(command);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var key = reader.GetString(0);
            var culture = await reader.IsDBNullAsync(1, cancellationToken).ConfigureAwait(false)
                ? null
                : reader.GetString(1);
            var value = reader.GetString(2);

            entries.Add(new TranslationEntry(key, culture, value));
        }

        return entries;
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        if (_initialized)
        {
            return;
        }

        await _initializationLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (_initialized)
            {
                return;
            }

            await using var connection = new SqlConnection(_options.ConnectionString);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

            if (_options.EnsureTableCreated)
            {
                await EnsureTableCreatedAsync(connection, cancellationToken).ConfigureAwait(false);
            }

            if (_options.SeedTranslations.Count > 0)
            {
                await SeedTranslationsAsync(connection, cancellationToken).ConfigureAwait(false);
            }

            _initialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    private async Task EnsureTableCreatedAsync(SqlConnection connection, CancellationToken cancellationToken)
    {
        var schema = EscapeIdentifier(_options.Schema);
        var table = EscapeIdentifier(_options.TableName);
        var qualifiedTable = $"{schema}.{table}";
        var indexName = EscapeIdentifier($"UX_{_options.TableName}_Key_Culture");

        var sql = $"""
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'{EscapeSqlLiteral(_options.Schema)}')
BEGIN
    EXEC(N'CREATE SCHEMA {schema}');
END;

IF OBJECT_ID(N'{EscapeSqlLiteral(_options.Schema)}.{EscapeSqlLiteral(_options.TableName)}', N'U') IS NULL
BEGIN
    CREATE TABLE {qualifiedTable}
    (
        [Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Key] NVARCHAR(450) NOT NULL,
        [Culture] NVARCHAR(32) NULL,
        [Value] NVARCHAR(MAX) NOT NULL,
        [CreatedAtUtc] DATETIME2 NOT NULL CONSTRAINT [DF_{_options.TableName}_CreatedAtUtc] DEFAULT SYSUTCDATETIME(),
        [UpdatedAtUtc] DATETIME2 NOT NULL CONSTRAINT [DF_{_options.TableName}_UpdatedAtUtc] DEFAULT SYSUTCDATETIME()
    );

    CREATE UNIQUE INDEX {indexName}
        ON {qualifiedTable} ([Key], [Culture]);
END;
""";

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        ApplyCommandTimeout(command);

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task SeedTranslationsAsync(SqlConnection connection, CancellationToken cancellationToken)
    {
        foreach (var translation in _options.SeedTranslations)
        {
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"""
IF NOT EXISTS
(
    SELECT 1
    FROM {GetQualifiedTableName()}
    WHERE [Key] = @Key
      AND (([Culture] IS NULL AND @Culture IS NULL) OR [Culture] = @Culture)
)
BEGIN
    INSERT INTO {GetQualifiedTableName()} ([Key], [Culture], [Value])
    VALUES (@Key, @Culture, @Value);
END;
""";

            ApplyCommandTimeout(command);

            command.Parameters.Add(new SqlParameter("@Key", System.Data.SqlDbType.NVarChar, 450)
            {
                Value = translation.Key
            });
            command.Parameters.Add(new SqlParameter("@Culture", System.Data.SqlDbType.NVarChar, 32)
            {
                Value = (object?)translation.Culture ?? DBNull.Value
            });
            command.Parameters.Add(new SqlParameter("@Value", System.Data.SqlDbType.NVarChar)
            {
                Value = translation.Value
            });

            await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private string GetQualifiedTableName()
    {
        return $"{EscapeIdentifier(_options.Schema)}.{EscapeIdentifier(_options.TableName)}";
    }

    private void ApplyCommandTimeout(SqlCommand command)
    {
        if (_options.CommandTimeoutSeconds.HasValue)
        {
            command.CommandTimeout = _options.CommandTimeoutSeconds.Value;
        }
    }

    private void ValidateOptions()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.ConnectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.Schema);
        ArgumentException.ThrowIfNullOrWhiteSpace(_options.TableName);

        if (_options.ReloadMode == TranslationReloadMode.Periodic &&
            (!_options.ReloadInterval.HasValue || _options.ReloadInterval.Value <= TimeSpan.Zero))
        {
            throw new InvalidOperationException(
                "ReloadInterval must be greater than zero when ReloadMode is Periodic.");
        }
    }

    private static string EscapeIdentifier(string name)
    {
        return $"[{name.Replace("]", "]]", StringComparison.Ordinal)}]";
    }

    private static string EscapeSqlLiteral(string value)
    {
        return value.Replace("'", "''", StringComparison.Ordinal);
    }
}