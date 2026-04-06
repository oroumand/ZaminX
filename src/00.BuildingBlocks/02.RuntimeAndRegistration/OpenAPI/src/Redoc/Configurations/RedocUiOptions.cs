namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc.Configurations;

public sealed class RedocUiOptions
{
    public const string SectionName = "Redoc";

    public bool Enabled { get; set; } = true;

    public string RoutePrefix { get; set; } = "redoc";

    public string DocumentTitle { get; set; } = "ReDoc";
}
