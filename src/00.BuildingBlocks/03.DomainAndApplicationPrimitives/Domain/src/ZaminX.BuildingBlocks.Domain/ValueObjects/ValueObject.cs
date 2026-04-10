namespace ZaminX.BuildingBlocks.Domain.ValueObjects;

public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
    where TValueObject : ValueObject<TValueObject>
{
    public bool Equals(TValueObject? other)
    {
        return this == other;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TValueObject other)
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(component => component?.GetHashCode() ?? 0)
            .Aggregate(0, (current, hashCode) => current ^ hashCode);
    }

    protected abstract IEnumerable<object?> GetEqualityComponents();

    public static bool operator ==(
        ValueObject<TValueObject>? left,
        ValueObject<TValueObject>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(
        ValueObject<TValueObject>? left,
        ValueObject<TValueObject>? right)
    {
        return !(left == right);
    }
}