using System.Collections;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class NotEmptyGuardClause
{
    public static void NotEmpty<T>(this Guard guard, T? value, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (value is null)
        {
            throw new InvalidOperationException(message);
        }

        if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
        {
            throw new InvalidOperationException(message);
        }

        if (value is ICollection collectionValue && collectionValue.Count == 0)
        {
            throw new InvalidOperationException(message);
        }

        if (value is IEnumerable enumerableValue)
        {
            var enumerator = enumerableValue.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException(message);
            }
        }

        if (EqualityComparer<T>.Default.Equals(value, default!))
        {
            throw new InvalidOperationException(message);
        }
    }

    public static void NotEmpty<T>(this Guard guard, T value, IEqualityComparer<T> equalityComparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        EnsureMessage(message);

        if (equalityComparer.Equals(value, default!))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
