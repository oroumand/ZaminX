using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Internals;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write.Services;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreBuilderExtensions
{
    public static EntityFrameworkCoreBuilder<TDbContext> AddWriteUnitOfWork<TDbContext>(
    this EntityFrameworkCoreBuilder<TDbContext> builder)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.TryAddScoped<IUnitOfWork, EfUnitOfWork<TDbContext>>();

        return builder;
    }

    public static EntityFrameworkCoreBuilder<TDbContext> ScanWriteRepositoriesFromAssemblies<TDbContext>(
        this EntityFrameworkCoreBuilder<TDbContext> builder,
        params Assembly[] assemblies)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assemblies);

        WriteRepositoryRegistrationHelper.RegisterRepositories(builder.Services, assemblies);

        return builder;
    }

}
