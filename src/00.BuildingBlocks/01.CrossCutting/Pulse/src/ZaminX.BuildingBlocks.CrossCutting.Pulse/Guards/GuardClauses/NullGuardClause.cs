namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class NullGuardClause
{
    public static void Null<T>(this Guard guard, T? value, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (value is not null)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
