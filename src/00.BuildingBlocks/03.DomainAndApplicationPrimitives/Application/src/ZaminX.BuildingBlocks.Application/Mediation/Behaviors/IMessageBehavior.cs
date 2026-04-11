using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public interface IMessageBehavior<in TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
}
