using System.Reflection;

namespace ZaminX.BuildingBlocks.Application.FluentValidation.Configurations;

public sealed class FluentValidationConfiguration
{
    private readonly List<Assembly> _assemblies = [];

    public IReadOnlyCollection<Assembly> Assemblies => _assemblies.AsReadOnly();

    public FluentValidationConfiguration AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        if (_assemblies.Contains(assembly))
            return this;

        _assemblies.Add(assembly);
        return this;
    }

    public FluentValidationConfiguration AddAssemblies(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
        {
            AddAssembly(assembly);
        }

        return this;
    }
}