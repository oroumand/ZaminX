using Newtonsoft.Json;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Exceptions;

namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft.Services;

public sealed class NewtonsoftJsonSerializer(JsonSerializerSettings settings) : IJsonSerializer
{
    private const string ProviderName = "Newtonsoft";
    private readonly JsonSerializerSettings _settings = settings ?? throw new ArgumentNullException(nameof(settings));

    public string Serialize<T>(T? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        try
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
        catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
        {
            throw new Abstractions.Exceptions.JsonSerializationException(
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
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
        catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
        {
            throw new Abstractions.Exceptions.JsonSerializationException(
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
            return JsonConvert.DeserializeObject(json, type, _settings);
        }
        catch (Exception ex) when (ex is JsonException || ex is NotSupportedException)
        {
            throw new Abstractions.Exceptions.JsonSerializationException(
                operation: "Deserialize",
                providerName: ProviderName,
                targetType: type,
                message: $"Deserialization failed with provider '{ProviderName}' for type '{type.FullName}'.",
                innerException: ex);
        }
    }
}