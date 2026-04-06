namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.WebApiSample.Services;

public sealed class RequestIdGenerator : IRequestIdGenerator
{
    public string Create() => $"req-{Guid.NewGuid():N}";
}
