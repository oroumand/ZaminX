namespace ZaminX.BuildingBlocks.Data.Abstractions.Services;

public interface IDataAuditContext
{
    string? UserId { get; }
    DateTimeOffset Now { get; }
}
