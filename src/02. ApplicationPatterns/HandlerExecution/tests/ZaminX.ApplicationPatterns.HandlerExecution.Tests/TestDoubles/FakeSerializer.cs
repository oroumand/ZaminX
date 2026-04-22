using System.Text.Json;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeSerializer : IJsonSerializer
{
    public string Serialize<T>(T? value)
    {
        return JsonSerializer.Serialize(value);
    }

    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public object? Deserialize(string json, Type type)
    {
        return JsonSerializer.Deserialize(json, type);
    }
}