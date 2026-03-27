using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ZaminX.Mapper.Abstractions.Contracts;
using ZaminX.Mapper.AutoMapper.Adapters;

namespace ZaminX.Mapper.AutoMapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZaminXAutoMapperAdapter(this IServiceCollection services)
    {
        services.AddAutoMapper(c=> { },typeof(ServiceCollectionExtensions).Assembly);
        services.AddTransient<IMapperAdapter, AutoMapperAdapter>();

        return services;
    }
}