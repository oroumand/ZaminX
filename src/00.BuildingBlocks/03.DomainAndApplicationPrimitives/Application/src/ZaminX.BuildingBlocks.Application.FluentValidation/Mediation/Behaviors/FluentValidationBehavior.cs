using FluentValidation;
using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Error = ZaminX.BuildingBlocks.Application.Results.Error;

namespace ZaminX.BuildingBlocks.Application.FluentValidation.Mediation.Behaviors;

public sealed class FluentValidationBehavior<TMessage, TResponse> : IOrderedMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public FluentValidationBehavior(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators ?? Enumerable.Empty<IValidator<TMessage>>();
    }

    public int Order => 200;

    public async Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(next);

        if (!_validators.Any())
            return await next();

        var errors = new List<Error>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(message, cancellationToken);

            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(error =>
                    new Error(
                        string.IsNullOrWhiteSpace(error.ErrorCode)
                            ? "validation.error"
                            : $"validation.{error.ErrorCode}",
                        error.ErrorMessage)));
            }
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
            ?? throw new InvalidOperationException(
                "Generic Result.Failure(IEnumerable<Error>) was not found.");

        var result = failureMethod.Invoke(null, new object[] { errors })
                     ?? throw new InvalidOperationException(
                         "Validation failure result was null.");

        return (TResponse)result;
    }
}