using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.ApplicationPatterns.HandlerExecution;

public sealed class ResultContext
{
    private readonly List<Error> _errors = [];

    public bool HasErrors => _errors.Count > 0;

    public IReadOnlyCollection<Error> Errors => _errors;

    public void AddError(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        _errors.Add(error);
    }

    public void AddErrors(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        foreach (var error in errors)
        {
            ArgumentNullException.ThrowIfNull(error);
            _errors.Add(error);
        }
    }

    public void AddError(string code, string message)
    {
        _errors.Add(new Error(code, message));
    }

    public Result ToResult()
    {
        return HasErrors
            ? Result.Failure(_errors)
            : Result.Success();
    }

    public Result<T> ToResult<T>(T data)
    {
        return HasErrors
            ? Result<T>.Failure(_errors)
            : Result<T>.Success(data);
    }
}