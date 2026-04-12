using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Internals;

namespace Microsoft.Extensions.DependencyInjection;

public static class EntityFrameworkCoreBuilderExtensions
{
    public static EntityFrameworkCoreBuilder<TDbContext> ScanReadRepositoriesFromAssemblies<TDbContext>(
    this EntityFrameworkCoreBuilder<TDbContext> builder,
    params Assembly[] assemblies)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(assemblies);


    ReadRepositoryRegistrationHelper.RegisterRepositories(builder.Services, assemblies);

        return builder;
    }


}
