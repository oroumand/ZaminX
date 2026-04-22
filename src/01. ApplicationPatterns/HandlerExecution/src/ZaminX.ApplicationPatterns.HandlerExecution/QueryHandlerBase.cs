using ZaminX.BuildingBlocks.Application.Queries;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;
using ZaminX.BuildingBlocks.Domain.Entities;

namespace ZaminX.ApplicationPatterns.HandlerExecution;

public abstract class QueryHandlerBase<TQuery, TResponse, TRepository, TEntity, TId> :
    ApplicationHandlerBase,
    IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TRepository : IReadRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    protected QueryHandlerBase(
        TRepository repository,
        HandlerServices services)
        : base(services)
    {
        ArgumentNullException.ThrowIfNull(repository);

        Repository = repository;
    }

    protected TRepository Repository { get; }

    public abstract Task<Result<TResponse>> Handle(
        TQuery query,
        CancellationToken cancellationToken = default);
}