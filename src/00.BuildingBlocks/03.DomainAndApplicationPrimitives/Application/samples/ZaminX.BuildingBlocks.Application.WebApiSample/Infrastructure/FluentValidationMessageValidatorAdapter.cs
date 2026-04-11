using FluentValidation;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Application.Validation;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Infrastructure;

public sealed class FluentValidationMessageValidatorAdapter<TMessage> : IMessageValidator<TMessage>
    where TMessage : IMessage
{
    private readonly IEnumerable<IValidator<TMessage>> _validators;

    public FluentValidationMessageValidatorAdapter(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators;
    }

    public async Task<IReadOnlyCollection<Error>> ValidateAsync(
        TMessage message,
        CancellationToken cancellationToken = default)
    {
        var errors = new List<Error>();

        foreach (var validator in _validators)
        {
            var result = await validator.ValidateAsync(message, cancellationToken);

            if (!result.IsValid)
            {
                errors.AddRange(
                    result.Errors.Select(x =>
                        new Error(
                            string.IsNullOrWhiteSpace(x.ErrorCode)
                                ? "validation.error"
                                : $"validation.{x.ErrorCode}",
                            x.ErrorMessage)));
            }
        }

        return errors;
    }
}