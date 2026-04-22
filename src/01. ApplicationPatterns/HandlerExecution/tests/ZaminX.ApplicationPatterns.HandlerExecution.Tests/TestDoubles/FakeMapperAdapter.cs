using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeMapperAdapter : IMapperAdapter
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        if (source is TDestination sameType)
            return sameType;

        return Activator.CreateInstance<TDestination>();
    }
}