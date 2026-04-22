using ZaminX.BuildingBlocks.Domain.Entities;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeAggregateRoot : AggregateRoot<int>
{
    public FakeAggregateRoot()
    {
    }

    public FakeAggregateRoot(int id) : base(id)
    {
    }
}