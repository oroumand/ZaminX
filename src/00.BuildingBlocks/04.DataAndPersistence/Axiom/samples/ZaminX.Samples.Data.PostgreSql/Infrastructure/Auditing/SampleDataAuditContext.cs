using ZaminX.BuildingBlocks.Data.Abstractions.Services;

namespace ZaminX.Samples.Data.PostgreSql.Infrastructure.Auditing;

public sealed class SampleDataAuditContext : IDataAuditContext
{
    public string? UserId => "sample-user";

    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}