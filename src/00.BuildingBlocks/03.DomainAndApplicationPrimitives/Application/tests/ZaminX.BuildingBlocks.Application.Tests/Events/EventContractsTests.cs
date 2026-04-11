namespace ZaminX.BuildingBlocks.Application.Events;

public class EventContractsTests
{
    private sealed class UserCreatedEvent : IEvent
    {
    }

    private sealed class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        public bool WasCalled { get; private set; }

        public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Event_Handler_Should_Be_Implementable()
    {
        var handler = new UserCreatedEventHandler();

        await handler.Handle(new UserCreatedEvent());

        Assert.True(handler.WasCalled);
    }
}