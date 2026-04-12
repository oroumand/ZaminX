namespace ZaminX.BuildingBlocks.Data.Abstractions.Services;

public sealed class DefaultDataAuditContext : IDataAuditContext
{
    public string? UserId => null;

    public DateTimeOffset Now => DateTimeOffset.UtcNow;

}