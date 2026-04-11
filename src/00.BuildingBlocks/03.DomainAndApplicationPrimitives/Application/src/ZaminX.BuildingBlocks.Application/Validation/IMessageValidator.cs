using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Validation;

public interface IMessageValidator<in TMessage>
    where TMessage : IMessage
{
    Task<IReadOnlyCollection<Error>> ValidateAsync(
        TMessage message,
        CancellationToken cancellationToken = default);
}