namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class EqualGuardClause
{
    public static void Equal<T>(this Guard guard, T value, T targetValue, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (!Equals(value, targetValue))
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void Equal<T>(this Guard guard, T value, T targetValue, IEqualityComparer<T> equalityComparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        EnsureMessage(message);

        if (!equalityComparer.Equals(value, targetValue))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
