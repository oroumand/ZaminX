namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class LessThanOrEqualGuardClause
{
    public static void LessThanOrEqual<T>(this Guard guard, T value, T maximumValue, IComparer<T> comparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(comparer);
        EnsureMessage(message);

        if (comparer.Compare(value, maximumValue) > 0)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void LessThanOrEqual<T>(this Guard guard, T value, T maximumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.LessThanOrEqual(value, maximumValue, Comparer<T>.Default, message);
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
