using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.Mapper.Abstractions.Contracts;
using ZaminX.Mapper.AutoMapper.Adapters;

namespace ZaminX.Mapper.AutoMapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXAutoMapperAdapter(
        this IServiceCollection services,
        Action<IMapperConfigurationExpression>? configure = null)
    {
        services.AddAutoMapper(configure ?? (_ => { }), typeof(ServiceCollectionExtensions).Assembly);
        services.AddTransient<IMapperAdapter, AutoMapperAdapter>();

        return services;
    }
}