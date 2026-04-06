namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.WebApiSample.Services;

using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Abstractions.Contracts;

public sealed class SystemClock : IClock, ISingletonDependency
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
