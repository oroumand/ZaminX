using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using ZaminX.BuildingBlocks.Data.Abstractions.Services;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXDataAccess(
    this IServiceCollection services,
    Action<DataAccessBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.TryAddSingleton<IDataAuditContext, DefaultDataAuditContext>();

        var builder = new DataAccessBuilder(services);

        configure(builder);

        return services;
    }

}
