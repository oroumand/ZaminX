using AutoMapper;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper.Adapters;

public sealed class AutoMapperAdapter(IMapper mapper) : IMapperAdapter
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return mapper.Map<TDestination>(source);
    }
}