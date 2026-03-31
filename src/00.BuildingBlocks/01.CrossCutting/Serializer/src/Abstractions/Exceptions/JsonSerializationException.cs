namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Exceptions;

public sealed class JsonSerializationException : Exception
{
    public JsonSerializationException(
        string operation,
        string providerName,
        Type? targetType,
        string message,
        Exception? innerException = null)
        : base(message, innerException)
    {
        Operation = operation;
        ProviderName = providerName;
        TargetType = targetType;
    }

    public string Operation { get; }

    public string ProviderName { get; }

    public Type? TargetType { get; }
}