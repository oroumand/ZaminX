using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper.Adapters;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper.Configurations;

namespace Microsoft.Extensions.DependencyInjection;

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