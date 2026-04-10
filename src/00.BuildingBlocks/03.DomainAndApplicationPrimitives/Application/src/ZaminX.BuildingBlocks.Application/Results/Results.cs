namespace ZaminX.BuildingBlocks.Application.Results;

public class Result
{
    private readonly Error[] _errors;

    protected Result(bool isSuccess, IEnumerable<Error>? errors)
    {
        var normalizedErrors = NormalizeErrors(errors);

        if (isSuccess && normalizedErrors.Length > 0)
            throw new InvalidOperationException("A successful result cannot contain errors.");

        if (!isSuccess && normalizedErrors.Length == 0)
            throw new InvalidOperationException("A failed result must contain at least one error.");

        IsSuccess = isSuccess;
        _errors = normalizedErrors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyCollection<Error> Errors => _errors;

    public Error? FirstError => _errors.FirstOrDefault();

    public static Result Success()
    {
        return new Result(true, []);
    }

    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result(false, [error]);
    }

    public static Result Failure(params Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return new Result(false, errors);
    }

    public static Result Failure(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return new Result(false, errors);
    }

    private static Error[] NormalizeErrors(IEnumerable<Error>? errors)
    {
        return (errors is null) ?
            [] :

            [.. errors
            .Select(error =>
            {
                ArgumentNullException.ThrowIfNull(error);
                return error;
            })];
    }
}