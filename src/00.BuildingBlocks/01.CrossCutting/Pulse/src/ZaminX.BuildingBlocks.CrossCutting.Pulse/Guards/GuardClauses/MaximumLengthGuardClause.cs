using System.Collections;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class MaximumLengthGuardClause
{
    public static void MaximumLength(this Guard guard, string value, int maximumLength, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Length > maximumLength)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void MaximumLength(this Guard guard, ICollection value, int maximumLength, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Count > maximumLength)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
