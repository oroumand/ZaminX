namespace ZaminX.BuildingBlocks.Application.Mediation;

internal sealed class ServiceProviderStub : IServiceProvider
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register(Type type, object instance)
    {
        _services[type] = instance;
    }

    public object? GetService(Type serviceType)
    {
        if (_services.TryGetValue(serviceType, out var service))
            return service;

        return null;
    }
}