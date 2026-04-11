using Microsoft.Extensions.Logging;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation.Behaviors;

public sealed class RequestTelemetryBehavior<TMessage, TResponse> : IOrderedMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    private readonly ILogger<RequestTelemetryBehavior<TMessage, TResponse>> _logger;

    public RequestTelemetryBehavior(ILogger<RequestTelemetryBehavior<TMessage, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public int Order => 100;

    public async Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(next);

        var messageType = typeof(TMessage).FullName ?? typeof(TMessage).Name;
        var startedAt = DateTimeOffset.UtcNow;

        _logger.LogInformation(
            "Handling request {RequestType} at {StartedAt}.",
            messageType,
            startedAt);

        var startTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();

        try
        {
            var response = await next();

            var elapsedMilliseconds = GetElapsedMilliseconds(startTimestamp);

            _logger.LogInformation(
                "Handled request {RequestType} in {ElapsedMilliseconds} ms. Success: {IsSuccess}. ErrorCount: {ErrorCount}.",
                messageType,
                elapsedMilliseconds,
                response.IsSuccess,
                response.Errors.Count);

            return response;
        }
        catch (Exception exception)
        {
            var elapsedMilliseconds = GetElapsedMilliseconds(startTimestamp);

            _logger.LogError(
                exception,
                "Request {RequestType} failed after {ElapsedMilliseconds} ms.",
                messageType,
                elapsedMilliseconds);

            throw;
        }
    }

    private static double GetElapsedMilliseconds(long startTimestamp)
    {
        var endTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        return (endTimestamp - startTimestamp) * 1000d / System.Diagnostics.Stopwatch.Frequency;
    }
}