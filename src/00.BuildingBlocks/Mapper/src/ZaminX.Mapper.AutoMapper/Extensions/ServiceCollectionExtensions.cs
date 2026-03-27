using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.Mapper.Abstractions.Contracts;
using ZaminX.Mapper.AutoMapper.Adapters;
using ZaminX.Mapper.AutoMapper.Configurations;

namespace ZaminX.Mapper.AutoMapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXAutoMapperAdapter(this IServiceCollection services)
    {
        return services.AddZaminXAutoMapperAdapter(_ => { });
    }

    public static IServiceCollection AddZaminXAutoMapperAdapter(
        this IServiceCollection services,
        Action<ZaminXAutoMapperOptions> configure)
    {
        var options = new ZaminXAutoMapperOptions();
        configure(options);

        var assemblies = options.Assemblies.Count > 0
            ? options.Assemblies.ToArray()
            : [typeof(ServiceCollectionExtensions).Assembly];

        services.AddAutoMapper(options.Configure ?? (_ => { }), assemblies);
        services.AddTransient<IMapperAdapter, AutoMapperAdapter>();

        return services;
    }
}