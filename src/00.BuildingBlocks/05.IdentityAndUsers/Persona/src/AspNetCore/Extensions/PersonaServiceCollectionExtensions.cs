using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Configurations;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class PersonaServiceCollectionExtensions
{
    public static IServiceCollection AddPersonaAspNetCore(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<PersonaAspNetCoreOptions>();
        RegisterCoreServices(services);

        return services;
    }

    public static IServiceCollection AddPersonaAspNetCore(
        this IServiceCollection services,
        Action<PersonaAspNetCoreOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.AddOptions<PersonaAspNetCoreOptions>()
            .Configure(configure);

        RegisterCoreServices(services);

        return services;
    }

    public static IServiceCollection AddPersonaAspNetCore(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Persona")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        services.AddOptions<PersonaAspNetCoreOptions>()
            .Bind(configuration.GetSection(sectionName));

        RegisterCoreServices(services);

        return services;
    }

    private static void RegisterCoreServices(IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.TryAddScoped<HttpContextCurrentUser>();
        services.TryAddScoped<ICurrentUser>(serviceProvider =>
            serviceProvider.GetRequiredService<HttpContextCurrentUser>());
        services.TryAddScoped<IWebCurrentUser>(serviceProvider =>
            serviceProvider.GetRequiredService<HttpContextCurrentUser>());
    }
}