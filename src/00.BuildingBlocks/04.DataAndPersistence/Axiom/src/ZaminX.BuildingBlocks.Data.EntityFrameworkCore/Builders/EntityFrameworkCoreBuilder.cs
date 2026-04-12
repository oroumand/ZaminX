using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Enums;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;

public sealed class EntityFrameworkCoreBuilder<TDbContext>
where TDbContext : DbContext
{
    private Action<IServiceProvider, DbContextOptionsBuilder>? _providerConfiguration;

    internal bool ProviderConfigured => _providerConfiguration is not null;

    internal DataAccessRegistrationKind RegistrationKind { get; }

    public IServiceCollection Services { get; }

    public EntityFrameworkCoreBuilder(
        IServiceCollection services,
        DataAccessRegistrationKind registrationKind)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
        RegistrationKind = registrationKind;
    }

    public void SetProvider(Action<IServiceProvider, DbContextOptionsBuilder> providerConfiguration)
    {
        ArgumentNullException.ThrowIfNull(providerConfiguration);

        if (ProviderConfigured)
        {
            throw new InvalidOperationException(
                $"An EF Core provider has already been configured for '{typeof(TDbContext).FullName}'.");
        }

        _providerConfiguration = providerConfiguration;
    }

    internal void ValidateProviderConfigured()
    {
        if (!ProviderConfigured)
        {
            throw new InvalidOperationException(
                $"No EF Core provider has been configured for '{typeof(TDbContext).FullName}'. " +
                "Call a provider extension such as WithSqlServer(...) or WithPostgreSql(...).");
        }
    }

    internal void Configure(IServiceProvider serviceProvider, DbContextOptionsBuilder optionsBuilder)
    {
        if (_providerConfiguration is null)
        {
            throw new InvalidOperationException(
                $"No EF Core provider has been configured for '{typeof(TDbContext).FullName}'.");
        }

        _providerConfiguration(serviceProvider, optionsBuilder);

        if (RegistrationKind == DataAccessRegistrationKind.Read)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    }


}
