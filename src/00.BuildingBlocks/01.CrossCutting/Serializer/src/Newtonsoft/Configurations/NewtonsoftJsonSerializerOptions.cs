using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft.Configurations;

public sealed class NewtonsoftJsonSerializerOptions
{
    public bool UseCamelCase { get; set; } = true;

    public bool WriteIndented { get; set; }

    internal JsonSerializerSettings ToJsonSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = UseCamelCase
                ? new CamelCasePropertyNamesContractResolver()
                : new DefaultContractResolver(),
            Formatting = WriteIndented
                ? Formatting.Indented
                : Formatting.None
        };
    }
}
