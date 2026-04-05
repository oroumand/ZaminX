namespace ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Exceptions;

public class StashXException : Exception
{
    public StashXException()
    {
    }

    public StashXException(string message)
        : base(message)
    {
    }

    public StashXException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}