namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Exceptions;

public sealed class ZaminXDependencyInjectionException : Exception
{
    public ZaminXDependencyInjectionException(string message)
        : base(message)
    {
    }
}
