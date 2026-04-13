namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class NotEqualGuardClause
{
    public static void NotEqual<T>(this Guard guard, T value, T targetValue, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (Equals(value, targetValue))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
