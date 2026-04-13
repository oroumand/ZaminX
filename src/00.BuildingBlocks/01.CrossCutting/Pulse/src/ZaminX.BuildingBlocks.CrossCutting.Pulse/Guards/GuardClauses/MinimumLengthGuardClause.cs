using System.Collections;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class MinimumLengthGuardClause
{
    public static void MinimumLength(this Guard guard, string value, int minimumLength, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Length < minimumLength)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void MinimumLength(this Guard guard, ICollection value, int minimumLength, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Count < minimumLength)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
