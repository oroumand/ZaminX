using System.Text.RegularExpressions;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class MatchGuardClause
{
    public static void Match(this Guard guard, string value, string pattern, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(pattern);
        EnsureMessage(message);

        if (!Regex.IsMatch(value, pattern, RegexOptions.None, TimeSpan.FromSeconds(3)))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
