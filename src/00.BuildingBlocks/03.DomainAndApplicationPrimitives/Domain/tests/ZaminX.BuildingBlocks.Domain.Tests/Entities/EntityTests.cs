using ZaminX.BuildingBlocks.Domain.Entities;

namespace ZaminX.BuildingBlocks.Domain.Tests.Entities;

public class EntityTests
{
    private class TestEntity(int id) : Entity<int>(id)
    {
    }

    private class AnotherEntity(int id) : Entity<int>(id)
    {
    }

    [Fact]
    public void Entities_With_Same_Id_And_Type_Should_Be_Equal()
    {
        var a = new TestEntity(1);
        var b = new TestEntity(1);

        Assert.True(a.Equals(b));
        Assert.True(a == b);
    }

    [Fact]
    public void Entities_With_Same_Id_But_Different_Type_Should_Not_Be_Equal()
    {
        var a = new TestEntity(1);
        var b = new AnotherEntity(1);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Transient_Entities_Should_Not_Be_Equal()
    {
        var a = new TestEntity(0);
        var b = new TestEntity(0);

        Assert.False(a.Equals(b));
    }

    [Fact]
    public void Same_Instance_Should_Be_Equal()
    {
        var a = new TestEntity(1);

        Assert.True(a.Equals(a));
    }
}
