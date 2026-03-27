using AutoMapper;
using System.Reflection;

namespace ZaminX.Mapper.AutoMapper.Configurations;

public sealed class ZaminXAutoMapperOptions
{
    public IList<Assembly> Assemblies { get; } = new List<Assembly>();

    public Action<IMapperConfigurationExpression>? Configure { get; set; }
}