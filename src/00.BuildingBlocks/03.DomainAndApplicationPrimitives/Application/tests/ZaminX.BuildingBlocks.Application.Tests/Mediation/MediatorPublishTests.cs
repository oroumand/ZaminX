using ZaminX.BuildingBlocks.Application.Events;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public class MediatorPublishTests
{
    private sealed class UserCreatedEvent : IEvent
    {
    }

    private sealed class FirstHandler : IEventHandler<UserCreatedEvent>
    {
        public bool WasCalled { get; private set; }

        public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class SecondHandler : IEventHandler<UserCreatedEvent>
    {
        public bool WasCalled { get; private set; }

        public Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task Publish_Should_Invoke_All_Event_Handlers()
    {
        var provider = new ServiceProviderStub();
        var first = new FirstHandler();
        var second = new SecondHandler();

        provider.Register(
            typeof(IEnumerable<IEventHandler<UserCreatedEvent>>),
            new IEventHandler<UserCreatedEvent>[] { first, second });

        var mediator = new Mediator(provider);

        await mediator.Publish(new UserCreatedEvent());

        Assert.True(first.WasCalled);
        Assert.True(second.WasCalled);
    }
}