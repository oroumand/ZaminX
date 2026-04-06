namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger.Configurations;

public sealed class SwaggerUiOptions
{
    public const string SectionName = "Swagger";

    public bool Enabled { get; set; } = true;

    public string RoutePrefix { get; set; } = "swagger";

    public string DocumentTitle { get; set; } = "Swagger UI";

    public string EndpointName { get; set; } = "OpenAPI";
}
