namespace ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;

public interface IMapperAdapter
{
    TDestination Map<TSource, TDestination>(TSource source);
}