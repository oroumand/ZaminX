using ZaminX.BuildingBlocks.Application.Events;
using ZaminX.BuildingBlocks.Application.Requests;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public interface IMediator
{
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        where TResponse : Result;

    Task Publish<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}