using ZaminX.BuildingBlocks.Application.Mediation;
using ZaminX.BuildingBlocks.Application.Messages;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.WebApiSample.Behaviors;

public sealed class HeaderLoggingBehavior<TMessage, TResponse> : IOrderedMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HeaderLoggingBehavior<TMessage, TResponse>> _logger;

    public HeaderLoggingBehavior(
        IHttpContextAccessor httpContextAccessor,
        ILogger<HeaderLoggingBehavior<TMessage, TResponse>> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public int Order => 500;

    public async Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default)
    {
        var requestId = _httpContextAccessor.HttpContext?.Request.Headers["X-Request-Id"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(requestId))
        {
            _logger.LogInformation(
                "Custom behavior observed X-Request-Id: {RequestId} for message {MessageType}",
                requestId,
                typeof(TMessage).Name);
        }

        return await next();
    }
}