namespace ZaminX.BuildingBlocks.Application.Configurations;

public sealed class RelayConfiguration
{
    private readonly List<Type> _openBehaviorTypes = [];

    public IReadOnlyCollection<Type> OpenBehaviorTypes => _openBehaviorTypes.AsReadOnly();

    public bool EnableRequestTelemetryBehavior { get; set; } = true;

    public bool EnableExceptionToResultBehavior { get; set; } = true;

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