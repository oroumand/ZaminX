using System;
using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Enums;

namespace Microsoft.Extensions.DependencyInjection;

public static class DataAccessBuilderExtensions
{
    public static DataAccessBuilder UseEntityFrameworkCore<TDbContext>(
    this DataAccessBuilder builder,
    Action<EntityFrameworkCoreBuilder<TDbContext>> configure)
    where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.MarkRegistration(DataAccessRegistrationKind.Write);

        var efBuilder = new EntityFrameworkCoreBuilder<TDbContext>(
            builder.Services,
            DataAccessRegistrationKind.Write);

        configure(efBuilder);

        efBuilder.ValidateProviderConfigured();

        builder.Services.AddDbContext<TDbContext>((serviceProvider, optionsBuilder) =>
        {
            efBuilder.Configure(serviceProvider, optionsBuilder);
        });

        return builder;
    }

    public static DataAccessBuilder UseReadEntityFrameworkCore<TDbContext>(
        this DataAccessBuilder builder,
        Action<EntityFrameworkCoreBuilder<TDbContext>> configure)
        where TDbContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configure);

        builder.MarkRegistration(DataAccessRegistrationKind.Read);

        var efBuilder = new EntityFrameworkCoreBuilder<TDbContext>(
            builder.Services,
            DataAccessRegistrationKind.Read);

        configure(efBuilder);

        efBuilder.ValidateProviderConfigured();

        builder.Services.AddDbContext<TDbContext>((serviceProvider, optionsBuilder) =>
        {
            efBuilder.Configure(serviceProvider, optionsBuilder);
        });

        return builder;
    }


}
