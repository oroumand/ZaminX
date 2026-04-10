using ZaminX.BuildingBlocks.Domain.Exceptions;

namespace ZaminX.BuildingBlocks.Domain.Tests.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_With_Code_Should_Set_Code_And_Message()
    {
        const string code = "order.invalid-state";

        var exception = new DomainException(code);

        Assert.Equal(code, exception.Code);
        Assert.Equal(code, exception.Message);
    }

    [Fact]
    public void Constructor_With_Code_And_Message_Should_Set_Properties()
    {
        const string code = "order.invalid-state";
        const string message = "Order is in invalid state.";

        var exception = new DomainException(code, message);

        Assert.Equal(code, exception.Code);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_With_Code_Message_And_InnerException_Should_Set_Properties()
    {
        const string code = "order.invalid-state";
        const string message = "Order is in invalid state.";
        var innerException = new InvalidOperationException("Inner exception.");

        var exception = new DomainException(code, message, innerException);

        Assert.Equal(code, exception.Code);
        Assert.Equal(message, exception.Message);
        Assert.Same(innerException, exception.InnerException);
    }

    [Fact]
    public void Constructor_Should_Throw_ArgumentNullException_When_Code_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new DomainException((string)null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_ArgumentException_When_Code_Is_Empty_Or_WhiteSpace(string invalidCode)
    {
        Assert.Throws<ArgumentException>(() => new DomainException(invalidCode));
    }
}