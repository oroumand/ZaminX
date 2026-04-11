using System.Reflection;

namespace ZaminX.BuildingBlocks.Application.Configurations;

public sealed class RelayConfiguration
{
    private readonly List<Assembly> _assemblies = [];
    private readonly List<Type> _openBehaviorTypes = [];

    public IReadOnlyCollection<Assembly> Assemblies => _assemblies.AsReadOnly();

    public IReadOnlyCollection<Type> OpenBehaviorTypes => _openBehaviorTypes.AsReadOnly();

    public bool EnableRequestTelemetryBehavior { get; set; } = true;

    public bool EnableValidationBehavior { get; set; } = true;

    public bool EnableExceptionToResultBehavior { get; set; } = true;

    public RelayConfiguration AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        if (_assemblies.Contains(assembly))
            return this;

        _assemblies.Add(assembly);
        return this;
    }

    public RelayConfiguration AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
        {
            AddAssembly(assembly);
        }

        return this;
    }

    public RelayConfiguration AddOpenBehavior(Type openBehaviorType)
    {
        ArgumentNullException.ThrowIfNull(openBehaviorType);

        if (!openBehaviorType.IsGenericTypeDefinition)
            throw new InvalidOperationException(
                $"Behavior type '{openBehaviorType.FullName}' must be an open generic type.");

        if (_openBehaviorTypes.Contains(openBehaviorType))
            return this;

        _openBehaviorTypes.Add(openBehaviorType);
        return this;
    }
}