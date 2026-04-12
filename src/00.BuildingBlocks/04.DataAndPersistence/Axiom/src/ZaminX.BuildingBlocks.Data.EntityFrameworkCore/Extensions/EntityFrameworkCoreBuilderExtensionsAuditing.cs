using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Interceptors;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreBuilderAuditingExtensions
{
    public static EntityFrameworkCoreBuilder<TDbContext> EnableAuditing<TDbContext>(
    this EntityFrameworkCoreBuilder<TDbContext> builder)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);


    builder.Services.TryAddScoped<AuditSaveChangesInterceptor>();

        builder.AddInterceptor<AuditSaveChangesInterceptor>();

        return builder;
    }


}
