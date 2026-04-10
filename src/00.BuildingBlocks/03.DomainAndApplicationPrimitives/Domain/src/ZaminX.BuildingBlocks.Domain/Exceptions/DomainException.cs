namespace ZaminX.BuildingBlocks.Domain.Exceptions;

public class DomainException : Exception
{
    public string Code { get; }

    public DomainException(string code)
        : base(code)
    {
        Code = GuardCode(code);
    }

    public DomainException(string code, string message)
        : base(message)
    {
        Code = GuardCode(code);
    }

    public DomainException(string code, string message, Exception innerException)
        : base(message, innerException)
    {
        Code = GuardCode(code);
    }

    private static string GuardCode(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return code;
    }
}