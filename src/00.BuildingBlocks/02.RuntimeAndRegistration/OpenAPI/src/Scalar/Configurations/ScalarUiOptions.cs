namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar.Configurations;

public sealed class ScalarUiOptions
{
    public const string SectionName = "Scalar";

    public bool Enabled { get; set; } = true;

    public string RoutePrefix { get; set; } = "/scalar";

    public string? Title { get; set; }
}
