using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.Validation;

namespace ZaminX.BuildingBlocks.Application.Mediation.Behaviors;

public sealed class ValidationBehavior<TMessage, TResponse> : IOrderedMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    private readonly IEnumerable<IMessageValidator<TMessage>> _validators;

    public ValidationBehavior(IEnumerable<IMessageValidator<TMessage>> validators)
    {
        _validators = validators ?? Enumerable.Empty<IMessageValidator<TMessage>>();
    }

    public int Order => 200;

    public async Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(next);

        var errors = new List<Error>();

        foreach (var validator in _validators)
        {
            var validationErrors = await validator.ValidateAsync(message, cancellationToken);

            if (validationErrors.Count > 0)
                errors.AddRange(validationErrors);
        }

        if (errors.Count == 0)
            return await next();

        return CreateFailure(errors);
    }

    private static TResponse CreateFailure(IReadOnlyCollection<Error> errors)
    {
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)(object)Result.Failure(errors);

        var valueType = typeof(TResponse).GetGenericArguments()[0];
        var failureMethod = typeof(Result<>)
            .MakeGenericType(valueType)
            .GetMethod(nameof(Result<int>.Failure), [typeof(IEnumerable<Error>)])
            ?? throw new InvalidOperationException("Generic Result.Failure(IEnumerable<Error>) was not found.");

        var result = failureMethod.Invoke(null, new object[] { errors })
                     ?? throw new InvalidOperationException("Validation failure result was null.");

        return (TResponse)result;
    }
}