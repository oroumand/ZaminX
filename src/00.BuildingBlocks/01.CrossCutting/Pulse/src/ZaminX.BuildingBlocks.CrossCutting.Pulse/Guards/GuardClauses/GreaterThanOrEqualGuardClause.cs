namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class GreaterThanOrEqualGuardClause
{
    public static void GreaterThanOrEqual<T>(this Guard guard, T value, T minimumValue, IComparer<T> comparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(comparer);
        EnsureMessage(message);

        if (comparer.Compare(value, minimumValue) < 0)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void GreaterThanOrEqual<T>(this Guard guard, T value, T minimumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.GreaterThanOrEqual(value, minimumValue, Comparer<T>.Default, message);
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
