using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Services;

internal sealed class SqlServerTranslationMissingKeyRegistrar(
    IOptions<ParrotSqlServerOptions> options) : ITranslationMissingKeyRegistrar
{
    private readonly ParrotSqlServerOptions _options = options.Value;

    public async Task RegisterIfNeededAsync(string key, CultureInfo? culture, CancellationToken cancellationToken = default)
    {
        if (!_options.RegisterMissingKeys || string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        await using var connection = new SqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

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
            Value = key
        });
        command.Parameters.Add(new SqlParameter("@Culture", System.Data.SqlDbType.NVarChar, 32)
        {
            Value = (object?)culture?.Name ?? DBNull.Value
        });
        command.Parameters.Add(new SqlParameter("@Value", System.Data.SqlDbType.NVarChar)
        {
            Value = string.Format(
                CultureInfo.InvariantCulture,
                _options.MissingKeyDefaultValueTemplate,
                key)
        });

        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
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

    private static string EscapeIdentifier(string name)
    {
        return $"[{name.Replace("]", "]]", StringComparison.Ordinal)}]";
    }
}