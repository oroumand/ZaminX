using System.Text.Json;

namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft.Configurations;

public sealed class MicrosoftJsonSerializerOptions
{
    public bool PropertyNameCaseInsensitive { get; set; } = true;

    public bool WriteIndented { get; set; }

    public JsonNamingPolicy? PropertyNamingPolicy { get; set; }

    internal JsonSerializerOptions ToJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = PropertyNameCaseInsensitive,
            WriteIndented = WriteIndented,
            PropertyNamingPolicy = PropertyNamingPolicy
        };
    }
}