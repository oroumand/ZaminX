using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Queries;

public class QueryHandlerContractsTests
{
    private sealed class GetOrderByIdQuery : IQuery<Guid>
    {
    }

    private sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, Guid>
    {
        public Task<Result<Guid>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Result<Guid>.Success(Guid.NewGuid()));
        }
    }

    [Fact]
    public async Task QueryHandler_Should_Be_Implementable()
    {
        var handler = new GetOrderByIdQueryHandler();
        var query = new GetOrderByIdQuery();

        var result = await handler.Handle(query);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
    }
}