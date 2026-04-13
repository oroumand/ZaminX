using System.Collections;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class LengthGuardClause
{
    public static void Length(this Guard guard, string value, int length, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Length != length)
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void Length(this Guard guard, ICollection value, int length, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        EnsureMessage(message);

        if (value.Count != length)
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
