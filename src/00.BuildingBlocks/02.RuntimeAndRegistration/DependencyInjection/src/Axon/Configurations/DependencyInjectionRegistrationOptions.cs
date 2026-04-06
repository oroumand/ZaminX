namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Axon.Configurations;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Abstractions.Models;

public sealed class DependencyInjectionRegistrationOptions
{
    private readonly HashSet<Assembly> _assemblies = [];
    private readonly HashSet<Assembly> _excludedAssemblies = [];
    private readonly HashSet<Type> _excludedTypes = [];
    private readonly List<string> _excludedNamespacePrefixes = [];

    public IReadOnlyCollection<Assembly> Assemblies => _assemblies;

    public IReadOnlyCollection<Assembly> ExcludedAssemblies => _excludedAssemblies;

    public IReadOnlyCollection<Type> ExcludedTypes => _excludedTypes;

    public IReadOnlyCollection<string> ExcludedNamespacePrefixes => _excludedNamespacePrefixes;

    public bool EnableMarkerRegistration { get; set; } = true;

    public bool EnableConventionRegistration { get; private set; }

    public ServiceLifetime ConventionalLifetime { get; private set; } = ServiceLifetime.Scoped;

    public bool RegisterOpenGenerics { get; set; } = true;

    public bool RegisterSelfWhenNoServiceTypeFound { get; set; }

    public DuplicateRegistrationBehavior DuplicateRegistrationBehavior { get; set; } = DuplicateRegistrationBehavior.Skip;

    public Func<Type, bool>? TypeFilter { get; set; }

    public void AddAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        _assemblies.Add(assembly);
    }

    public void AddAssemblies(params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        foreach (var assembly in assemblies)
        {
            AddAssembly(assembly);
        }
    }

    public void AddAssemblyContaining<T>() => AddAssembly(typeof(T).Assembly);

    public void ExcludeAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        _excludedAssemblies.Add(assembly);
    }

    public void ExcludeType<T>() => _excludedTypes.Add(typeof(T));

    public void ExcludeType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        _excludedTypes.Add(type);
    }

    public void ExcludeNamespacePrefix(string namespacePrefix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(namespacePrefix);
        _excludedNamespacePrefixes.Add(namespacePrefix);
    }

    public void EnableConventionalRegistration(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        EnableConventionRegistration = true;
        ConventionalLifetime = lifetime;
    }
}
