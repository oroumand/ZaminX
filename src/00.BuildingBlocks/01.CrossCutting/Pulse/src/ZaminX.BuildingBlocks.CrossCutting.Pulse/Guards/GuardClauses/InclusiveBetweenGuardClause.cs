namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class InclusiveBetweenGuardClause
{
    public static void InclusiveBetween<T>(this Guard guard, T value, T minimumValue, T maximumValue, IComparer<T> comparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(comparer);
        EnsureMessage(message);

        if (comparer.Compare(value, minimumValue) < 0 || comparer.Compare(value, maximumValue) > 0)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void InclusiveBetween<T>(this Guard guard, T value, T minimumValue, T maximumValue, string message)
        where T : IComparable<T>, IComparable
    {
        guard.InclusiveBetween(value, minimumValue, maximumValue, Comparer<T>.Default, message);
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
