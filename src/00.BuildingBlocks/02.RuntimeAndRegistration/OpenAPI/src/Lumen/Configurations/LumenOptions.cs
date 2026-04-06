namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen.Configurations;

public sealed class LumenOptions
{
    public const string DefaultSectionName = "OpenApi";

    public bool Enabled { get; set; } = true;

    public string DefaultDocumentName { get; set; } = "v1";

    public string DocumentPath { get; set; } = "/openapi/{documentName}.json";
}
