using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Tests.Results;

public class ErrorTests
{
    [Fact]
    public void Constructor_Should_Set_Properties()
    {
        var error = new Error("orders.invalid-state", "Order is in invalid state.");

        Assert.Equal("orders.invalid-state", error.Code);
        Assert.Equal("Order is in invalid state.", error.Message);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Code_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Error(null!, "Message"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Code_Is_Empty_Or_WhiteSpace(string invalidCode)
    {
        Assert.Throws<ArgumentException>(() => new Error(invalidCode, "Message"));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Message_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Error("orders.invalid-state", null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Message_Is_Empty_Or_WhiteSpace(string invalidMessage)
    {
        Assert.Throws<ArgumentException>(() => new Error("orders.invalid-state", invalidMessage));
    }
}