namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.WebApiSample.Services;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
