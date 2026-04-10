using ZaminX.BuildingBlocks.Domain.ValueObjects;

namespace ZaminX.BuildingBlocks.Domain.Tests.ValueObjects;

public class ValueObjectTests
{
    private sealed class Money : ValueObject<Money>
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }

    private sealed class Distance : ValueObject<Distance>
    {
        public int Meters { get; }

        public Distance(int meters)
        {
            Meters = meters;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Meters;
        }
    }

    [Fact]
    public void ValueObjects_With_Same_Type_And_Same_Components_Should_Be_Equal()
    {
        var left = new Money(100, "USD");
        var right = new Money(100, "USD");

        Assert.True(left.Equals(right));
        Assert.True(left.Equals((object)right));
        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void ValueObjects_With_Same_Type_And_Different_Components_Should_Not_Be_Equal()
    {
        var left = new Money(100, "USD");
        var right = new Money(200, "USD");

        Assert.False(left.Equals(right));
        Assert.False(left.Equals((object)right));
        Assert.False(left == right);
        Assert.True(left != right);
    }

    [Fact]
    public void ValueObjects_With_Null_Should_Not_Be_Equal()
    {
        var valueObject = new Money(100, "USD");

        Assert.False(valueObject.Equals((Money?)null));
        Assert.False(valueObject == null);
        Assert.True(valueObject != null);
    }

    [Fact]
    public void Both_Null_ValueObjects_Should_Be_Equal()
    {
        Money? left = null;
        Money? right = null;

        Assert.True(left == right);
        Assert.False(left != right);
    }

    [Fact]
    public void Same_Instance_Should_Be_Equal()
    {
        var valueObject = new Money(100, "USD");

        Assert.True(valueObject.Equals(valueObject));
    }

    [Fact]
    public void Equal_ValueObjects_Should_Have_Same_HashCode()
    {
        var left = new Money(100, "USD");
        var right = new Money(100, "USD");

        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Fact]
    public void Different_ValueObjects_Should_Not_Be_Equal()
    {
        var left = new Distance(10);
        var right = new Distance(20);

        Assert.False(left.Equals(right));
    }
}