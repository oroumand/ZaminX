using System.Collections;

namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards.GuardClauses;

public static class EmptyGuardClause
{
    public static void Empty<T>(this Guard guard, T? value, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        EnsureMessage(message);

        if (value is null)
        {
            return;
        }

        if (value is string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return;
            }

            throw new InvalidOperationException(message);
        }

        if (value is ICollection collectionValue)
        {
            if (collectionValue.Count == 0)
            {
                return;
            }

            throw new InvalidOperationException(message);
        }

        if (value is IEnumerable enumerableValue)
        {
            var enumerator = enumerableValue.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return;
            }

            throw new InvalidOperationException(message);
        }

        if (EqualityComparer<T>.Default.Equals(value, default))
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    public static void Empty<T>(this Guard guard, T value, IEqualityComparer<T> equalityComparer, string message)
    {
        ArgumentNullException.ThrowIfNull(guard);
        ArgumentNullException.ThrowIfNull(equalityComparer);
        EnsureMessage(message);

        if (!equalityComparer.Equals(value, default!))
        {
            throw new InvalidOperationException(message);
        }
    }

    private static void EnsureMessage(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
    }
}
