using ZaminX.BuildingBlocks.Domain.Events;

namespace ZaminX.BuildingBlocks.Domain.Tests.Events;

public class IDomainEventTests
{
    private sealed class TestDomainEvent : IDomainEvent
    {
    }

    [Fact]
    public void Domain_Event_Should_Be_Implementable()
    {
        var domainEvent = new TestDomainEvent();

        Assert.NotNull(domainEvent);
        Assert.IsType<IDomainEvent>(domainEvent, exactMatch: false);
    }
}