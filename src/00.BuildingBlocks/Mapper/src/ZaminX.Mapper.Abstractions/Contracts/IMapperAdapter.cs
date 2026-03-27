namespace ZaminX.Mapper.Abstractions.Contracts;

public interface IMapperAdapter
{
    TDestination Map<TSource, TDestination>(TSource source);
}