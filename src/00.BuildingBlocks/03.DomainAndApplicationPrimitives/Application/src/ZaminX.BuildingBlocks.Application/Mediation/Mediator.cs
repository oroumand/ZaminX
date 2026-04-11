using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Events;
using ZaminX.BuildingBlocks.Application.Queries;
using ZaminX.BuildingBlocks.Application.Requests;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Mediation;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        where TResponse : Result
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var handler = ResolveRequestHandler(requestType, responseType);
        var behaviors = ResolveBehaviors(requestType, responseType);

        MessageHandlerDelegate<TResponse> next = () =>
            InvokeRequestHandler<TResponse>(handler, request, cancellationToken);

        foreach (var behavior in behaviors.Reverse())
        {
            var currentBehavior = behavior;
            var currentNext = next;

            next = () => InvokeBehavior(
                currentBehavior,
                request,
                currentNext,
                cancellationToken);
        }

        return await next();
    }

    public async Task Publish<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        var handlers = ResolveEventHandlers<TEvent>();

        foreach (var handler in handlers)
        {
            await handler.Handle(@event, cancellationToken);
        }
    }

    private object ResolveRequestHandler(Type requestType, Type responseType)
    {
        Type handlerType;

        if (typeof(ICommand).IsAssignableFrom(requestType) && responseType == typeof(Result))
        {
            handlerType = typeof(ICommandHandler<>).MakeGenericType(requestType);
        }
        else if (ImplementsGenericInterface(requestType, typeof(ICommand<>)))
        {
            var valueType = GetResultValueType(responseType);
            handlerType = typeof(ICommandHandler<,>).MakeGenericType(requestType, valueType);
        }
        else if (ImplementsGenericInterface(requestType, typeof(IQuery<>)))
        {
            var valueType = GetResultValueType(responseType);
            handlerType = typeof(IQueryHandler<,>).MakeGenericType(requestType, valueType);
        }
        else
        {
            throw new InvalidOperationException(
                $"Request type '{requestType.FullName}' is not a supported command or query.");
        }

        return _serviceProvider.GetService(handlerType)
               ?? throw new InvalidOperationException(
                   $"No handler was registered for request type '{requestType.FullName}'.");
    }

    private IEnumerable<object> ResolveBehaviors(Type messageType, Type responseType)
    {
        var behaviorType = typeof(IMessageBehavior<,>).MakeGenericType(messageType, responseType);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(behaviorType);

        var resolved = _serviceProvider.GetService(enumerableType) as IEnumerable<object>
                       ?? Enumerable.Empty<object>();

        return resolved
            .Select((instance, index) => new
            {
                Instance = instance,
                Order = GetBehaviorOrder(instance),
                RegistrationIndex = index
            })
            .OrderBy(x => x.Order)
            .ThenBy(x => x.RegistrationIndex)
            .Select(x => x.Instance)
            .ToArray();
    }

    private static int GetBehaviorOrder(object behavior)
    {
        var orderedInterface = behavior.GetType()
            .GetInterfaces()
            .FirstOrDefault(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IOrderedMessageBehavior<,>));

        if (orderedInterface is null)
            return 1000;

        var orderProperty = orderedInterface.GetProperty(nameof(IOrderedMessageBehavior<IRequest<Result>, Result>.Order))
                           ?? throw new InvalidOperationException("Ordered behavior did not expose Order.");

        return (int)(orderProperty.GetValue(behavior)
                     ?? throw new InvalidOperationException("Ordered behavior Order was null."));
    }

    private IEnumerable<IEventHandler<TEvent>> ResolveEventHandlers<TEvent>()
        where TEvent : IEvent
    {
        var enumerableType = typeof(IEnumerable<IEventHandler<TEvent>>);

        return _serviceProvider.GetService(enumerableType) as IEnumerable<IEventHandler<TEvent>>
               ?? Enumerable.Empty<IEventHandler<TEvent>>();
    }

    private static async Task<TResponse> InvokeRequestHandler<TResponse>(
        object handler,
        object request,
        CancellationToken cancellationToken)
        where TResponse : Result
    {
        var method = handler.GetType().GetMethod("Handle")
                     ?? throw new InvalidOperationException(
                         $"Handler '{handler.GetType().FullName}' does not contain Handle.");

        var taskObject = method.Invoke(handler, new[] { request, cancellationToken })
                         ?? throw new InvalidOperationException(
                             $"Handler '{handler.GetType().FullName}' returned null.");

        return await AwaitTaskResult<TResponse>(taskObject);
    }

    private static async Task<TResponse> InvokeBehavior<TResponse>(
        object behavior,
        object message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
        where TResponse : Result
    {
        var method = behavior.GetType().GetMethod("Handle")
                     ?? throw new InvalidOperationException(
                         $"Behavior '{behavior.GetType().FullName}' does not contain Handle.");

        var taskObject = method.Invoke(behavior, new object[] { message, next, cancellationToken })
                         ?? throw new InvalidOperationException(
                             $"Behavior '{behavior.GetType().FullName}' returned null.");

        return await AwaitTaskResult<TResponse>(taskObject);
    }

    private static async Task<TResponse> AwaitTaskResult<TResponse>(object taskObject)
        where TResponse : Result
    {
        var task = (Task)taskObject;
        await task.ConfigureAwait(false);

        var resultProperty = taskObject.GetType().GetProperty("Result")
                            ?? throw new InvalidOperationException("Task result property was not found.");

        var result = resultProperty.GetValue(taskObject)
                     ?? throw new InvalidOperationException("Task result was null.");

        return (TResponse)result;
    }

    private static bool ImplementsGenericInterface(Type type, Type genericInterfaceDefinition)
    {
        return type.GetInterfaces().Any(x =>
            x.IsGenericType &&
            x.GetGenericTypeDefinition() == genericInterfaceDefinition);
    }

    private static Type GetResultValueType(Type responseType)
    {
        if (!responseType.IsGenericType || responseType.GetGenericTypeDefinition() != typeof(Result<>))
            throw new InvalidOperationException(
                $"Response type '{responseType.FullName}' is not a generic result.");

        return responseType.GetGenericArguments()[0];
    }
}