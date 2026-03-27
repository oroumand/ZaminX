using AutoMapper;
using ZaminX.Mapper.Abstractions.Contracts;

namespace ZaminX.Mapper.AutoMapper.Adapters;

public sealed class AutoMapperAdapter(IMapper mapper) : IMapperAdapter
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return mapper.Map<TDestination>(source);
    }
}