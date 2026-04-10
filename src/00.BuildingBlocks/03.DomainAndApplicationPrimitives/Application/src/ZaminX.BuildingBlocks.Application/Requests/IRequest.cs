using ZaminX.BuildingBlocks.Application.Messages;

namespace ZaminX.BuildingBlocks.Application.Requests;

public interface IRequest<out TResponse> : IMessage
{
}