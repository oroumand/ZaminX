namespace ZaminX.BuildingBlocks.Application.Results;

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(TValue value)
        : base(true, Array.Empty<Error>())
    {
        _value = value;
    }

    private Result(IEnumerable<Error> errors)
        : base(false, errors)
    {
        _value = default;
    }

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("A failed result does not have a value.");

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static new Result<TValue> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<TValue>([error]);
    }

    public static new Result<TValue> Failure(params Error[] errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return new Result<TValue>(errors);
    }

    public static new Result<TValue> Failure(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        return new Result<TValue>(errors);
    }
}