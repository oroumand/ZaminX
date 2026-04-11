using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public interface IOrderedMessageBehavior<in TMessage, TResponse> : IMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    int Order { get; }
}