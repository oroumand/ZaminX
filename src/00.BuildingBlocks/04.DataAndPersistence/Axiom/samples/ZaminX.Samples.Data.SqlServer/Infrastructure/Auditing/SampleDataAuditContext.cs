using ZaminX.BuildingBlocks.Data.Abstractions.Services;

namespace ZaminX.Samples.Data.SqlServer.Infrastructure.Auditing;

public sealed class SampleDataAuditContext : IDataAuditContext
{
    public string? UserId => "sample-user";

    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}