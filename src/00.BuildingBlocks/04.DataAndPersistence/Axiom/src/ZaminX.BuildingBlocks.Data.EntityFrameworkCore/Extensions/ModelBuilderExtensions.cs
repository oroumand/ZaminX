using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Constants;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder AddAuditShadowProperties(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned())
            {
                continue;
            }

            if (entityType.FindPrimaryKey() is null)
            {
                continue;
            }

            var entityBuilder = modelBuilder.Entity(entityType.ClrType);

            entityBuilder.Property<DateTimeOffset>(AuditPropertyNames.CreatedAt);
            entityBuilder.Property<string?>(AuditPropertyNames.CreatedBy).HasMaxLength(200);
            entityBuilder.Property<DateTimeOffset>(AuditPropertyNames.ModifiedAt);
            entityBuilder.Property<string?>(AuditPropertyNames.ModifiedBy).HasMaxLength(200);
        }

        return modelBuilder;
    }


}
