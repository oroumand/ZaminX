using ZaminX.BuildingBlocks.Domain.Entities;
using ZaminX.BuildingBlocks.Domain.Events;

namespace ZaminX.BuildingBlocks.Domain.Tests.Entities;

public class AggregateRootTests
{
    private sealed class TestDomainEvent : IDomainEvent
    {
    }

    private sealed class TestAggregateRoot(int id) : AggregateRoot<int>(id)
    {
        public void Raise(IDomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }
    }

    [Fact]
    public void AggregateRoot_Should_Implement_IAggregateRoot()
    {
        var aggregateRoot = new TestAggregateRoot(1);

        Assert.IsAssignableFrom<IAggregateRoot>(aggregateRoot);
    }

    [Fact]
    public void AggregateRoot_Should_Start_With_Empty_DomainEvents()
    {
        var aggregateRoot = new TestAggregateRoot(1);

        Assert.Empty(aggregateRoot.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_Should_Add_Event_To_Collection()
    {
        var aggregateRoot = new TestAggregateRoot(1);
        var domainEvent = new TestDomainEvent();

        aggregateRoot.Raise(domainEvent);

        Assert.Single(aggregateRoot.DomainEvents);
        Assert.Contains(domainEvent, aggregateRoot.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_Should_Remove_All_Events()
    {
        var aggregateRoot = new TestAggregateRoot(1);

        aggregateRoot.Raise(new TestDomainEvent());
        aggregateRoot.Raise(new TestDomainEvent());

        aggregateRoot.ClearDomainEvents();

        Assert.Empty(aggregateRoot.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_Should_Throw_When_Event_Is_Null()
    {
        var aggregateRoot = new TestAggregateRoot(1);

        Assert.Throws<ArgumentNullException>(() => aggregateRoot.Raise(null!));
    }
}