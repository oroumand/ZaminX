namespace ZaminX.BuildingBlocks.Application.Events;

public interface IEventHandler<in TEvent>
    where TEvent : IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken = default);
}