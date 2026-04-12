using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ZaminX.BuildingBlocks.Data.Abstractions.Services;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Constants;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Interceptors;

public sealed class AuditSaveChangesInterceptor(IDataAuditContext auditContext) : SaveChangesInterceptor
{
    private readonly IDataAuditContext _auditContext = auditContext ?? throw new ArgumentNullException(nameof(auditContext));

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplyAuditValues(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditValues(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ApplyAuditValues(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var now = _auditContext.Now;
        var userId = _auditContext.UserId;

        foreach (var entry in dbContext.ChangeTracker.Entries())
        {
            if (!ShouldAudit(entry))
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                SetPropertyIfExists(entry, AuditPropertyNames.CreatedAt, now);
                SetPropertyIfExists(entry, AuditPropertyNames.CreatedBy, userId);
            }

            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                SetPropertyIfExists(entry, AuditPropertyNames.ModifiedAt, now);
                SetPropertyIfExists(entry, AuditPropertyNames.ModifiedBy, userId);
            }
        }
    }

    private static bool ShouldAudit(EntityEntry entry)
    {
        if (entry.State is not EntityState.Added and not EntityState.Modified)
        {
            return false;
        }

        if (entry.Metadata.IsOwned())
        {
            return false;
        }

        if (entry.Metadata.FindPrimaryKey() is null)
        {
            return false;
        }

        return true;
    }

    private static void SetPropertyIfExists(EntityEntry entry, string propertyName, object? value)
    {
        var property = entry.Metadata.FindProperty(propertyName);

        if (property is null)
        {
            return;
        }

        entry.Property(propertyName).CurrentValue = value;
    }


}
