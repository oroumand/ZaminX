using System.Text.Json;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Exceptions;

namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft.Services;

public sealed class MicrosoftJsonSerializer(JsonSerializerOptions options) : IJsonSerializer
{
    private const string ProviderName = "Microsoft";
    private readonly JsonSerializerOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    public string Serialize<T>(T? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        try
        {
            return JsonSerializer.Serialize(value, _options);
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is JsonException)
        {
            throw new JsonSerializationException(
                operation: "Serialize",
                providerName: ProviderName,
                targetType: typeof(T),
                message: $"Serialization failed with provider '{ProviderName}' for type '{typeof(T).FullName}'.",
                innerException: ex);
        }
    }

    public T? Deserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(json, _options);
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is JsonException)
        {
            throw new JsonSerializationException(
                operation: "Deserialize",
                providerName: ProviderName,
                targetType: typeof(T),
                message: $"Deserialization failed with provider '{ProviderName}' for type '{typeof(T).FullName}'.",
                innerException: ex);
        }
    }

    public object? Deserialize(string json, Type type)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        ArgumentNullException.ThrowIfNull(type);

        try
        {
            return JsonSerializer.Deserialize(json, type, _options);
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is JsonException)
        {
            throw new JsonSerializationException(
                operation: "Deserialize",
                providerName: ProviderName,
                targetType: type,
                message: $"Deserialization failed with provider '{ProviderName}' for type '{type.FullName}'.",
                innerException: ex);
        }
    }
}