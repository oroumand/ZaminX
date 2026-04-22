using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Results;
using ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;
using ZaminX.BuildingBlocks.Domain.Entities;


namespace ZaminX.ApplicationPatterns.HandlerExecution;

public abstract class CommandHandlerBase<TCommand, TRepository, TAggregate, TId> :
    ApplicationHandlerBase,
    ICommandHandler<TCommand>
    where TCommand : ICommand
    where TRepository : IWriteRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    protected CommandHandlerBase(
        TRepository repository,
        IUnitOfWork unitOfWork,
        HandlerServices services)
        : base(services)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        Repository = repository;
        UnitOfWork = unitOfWork;
    }

    protected TRepository Repository { get; }

    protected IUnitOfWork UnitOfWork { get; }

    public abstract Task<Result> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}

public abstract class CommandHandlerBase<TCommand, TResponse, TRepository, TAggregate, TId> :
    ApplicationHandlerBase,
    ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TRepository : IWriteRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    protected CommandHandlerBase(
        TRepository repository,
        IUnitOfWork unitOfWork,
        HandlerServices services)
        : base(services)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(unitOfWork);

        Repository = repository;
        UnitOfWork = unitOfWork;
    }

    protected TRepository Repository { get; }

    protected IUnitOfWork UnitOfWork { get; }

    public abstract Task<Result<TResponse>> Handle(
        TCommand command,
        CancellationToken cancellationToken = default);
}