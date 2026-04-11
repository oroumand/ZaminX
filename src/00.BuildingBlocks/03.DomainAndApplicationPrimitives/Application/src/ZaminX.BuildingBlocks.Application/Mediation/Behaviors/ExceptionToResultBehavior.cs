using Microsoft.Extensions.Logging;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation.Behaviors;

public sealed class ExceptionToResultBehavior<TMessage, TResponse> : IOrderedMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    private readonly ILogger<ExceptionToResultBehavior<TMessage, TResponse>> _logger;

    public ExceptionToResultBehavior(ILogger<ExceptionToResultBehavior<TMessage, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public int Order => 900;

    public async Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(next);

        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            var error = CreateError(exception);

            _logger.LogError(
                exception,
                "Exception was converted to result failure for message {MessageType}. ErrorCode: {ErrorCode}.",
                typeof(TMessage).FullName ?? typeof(TMessage).Name,
                error.Code);

            return CreateFailure(error);
        }
    }

    private static Error CreateError(Exception exception)
    {
        var codeProperty = exception.GetType().GetProperty("Code");

        if (codeProperty?.PropertyType == typeof(string))
        {
            var codeValue = codeProperty.GetValue(exception) as string;

            if (!string.IsNullOrWhiteSpace(codeValue))
                return new Error(codeValue, exception.Message);
        }

        return new Error("application.unhandled-exception", exception.Message);
    }

    private static TResponse CreateFailure(Error error)
    {
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        var valueType = typeof(TResponse).GetGenericArguments()[0];
        var failureMethod = typeof(Result<>)
            .MakeGenericType(valueType)
            .GetMethod(nameof(Result<int>.Failure), [typeof(Error)])
            ?? throw new InvalidOperationException("Generic Result.Failure(Error) was not found.");

        var result = failureMethod.Invoke(null, new object[] { error })
                     ?? throw new InvalidOperationException("Exception failure result was null.");

        return (TResponse)result;
    }
}