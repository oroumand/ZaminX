using Microsoft.Extensions.Logging;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution;

public abstract class ApplicationHandlerBase
{
    private readonly Lazy<ILogger> _logger;
    private ResultContext? _resultContext;

    protected ApplicationHandlerBase(HandlerServices services)
    {
        ArgumentNullException.ThrowIfNull(services);

        Services = services;
        _logger = new Lazy<ILogger>(
            () => services.LoggerFactory.CreateLogger(GetType()),
            LazyThreadSafetyMode.None);
    }

    protected HandlerServices Services { get; }

    protected IMapperAdapter Mapper => Services.Mapper;

    protected IJsonSerializer Serializer => Services.Serializer;

    protected ITranslator Translator => Services.Translator;

    protected ICurrentUser? CurrentUser => Services.CurrentUser;

    protected ILogger Logger => _logger.Value;

    protected ResultContext ResultContext => _resultContext ??= new ResultContext();

    protected bool HasErrors => _resultContext is { HasErrors: true };

    protected Result Success()
    {
        return Result.Success();
    }

    protected Result<T> Success<T>(T data)
    {
        return Result<T>.Success(data);
    }

    protected Result Invalid(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return Result.Failure(error);
    }

    protected Result Invalid(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return Result.Failure(errors);
    }

    protected Result Invalid(string code, string message)
    {
        return Result.Failure(new Error(code, message));
    }

    protected Result<T> Invalid<T>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return Result<T>.Failure(error);
    }

    protected Result<T> Invalid<T>(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return Result<T>.Failure(errors);
    }

    protected Result<T> Invalid<T>(string code, string message)
    {
        return Result<T>.Failure(new Error(code, message));
    }

    protected Result NotFound(string code, string message)
    {
        return Result.Failure(new Error(code, message));
    }

    protected Result<T> NotFound<T>(string code, string message)
    {
        return Result<T>.Failure(new Error(code, message));
    }

    protected Result Conflict(string code, string message)
    {
        return Result.Failure(new Error(code, message));
    }

    protected Result<T> Conflict<T>(string code, string message)
    {
        return Result<T>.Failure(new Error(code, message));
    }

    protected Result Forbidden(string code, string message)
    {
        return Result.Failure(new Error(code, message));
    }

    protected Result<T> Forbidden<T>(string code, string message)
    {
        return Result<T>.Failure(new Error(code, message));
    }

    protected Result Error(string code, string message)
    {
        return Result.Failure(new Error(code, message));
    }

    protected Result<T> Error<T>(string code, string message)
    {
        return Result<T>.Failure(new Error(code, message));
    }

    protected Result FailureFromContext()
    {
        return ResultContext.ToResult();
    }

    protected Result<T> FailureFromContext<T>(T data)
    {
        return ResultContext.ToResult(data);
    }
}