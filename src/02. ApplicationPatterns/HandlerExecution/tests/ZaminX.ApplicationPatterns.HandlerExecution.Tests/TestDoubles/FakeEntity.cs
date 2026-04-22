using ZaminX.BuildingBlocks.Domain.Entities;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeEntity : Entity<int>
{
    public FakeEntity()
    {
    }

    public FakeEntity(int id) : base(id)
    {
    }
}