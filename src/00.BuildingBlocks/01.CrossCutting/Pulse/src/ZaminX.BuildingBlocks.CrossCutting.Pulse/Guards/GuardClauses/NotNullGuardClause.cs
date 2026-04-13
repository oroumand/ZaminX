namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class NotNullGuardClause
{
    public static void NotNull<T>(this Guard guard, T? value, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (value is null)
        {
            throw new InvalidOperationException(message);
        }
    }

    // Backward-compatible alias for the old mistaken method name.
    public static void Null<T>(this Guard guard, T? value, string message)
    {
        guard.NotNull(value, message);
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
