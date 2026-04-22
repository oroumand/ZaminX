using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;
using ZaminX.BuildingBlocks.Application.Commands;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests;

public class CommandHandlerBaseTests
{
    [Fact]
    public async Task Non_generic_handler_should_expose_repository_and_unit_of_work()
    {
        var repository = new FakeWriteRepository();
        var unitOfWork = new FakeUnitOfWork();
        var services = CreateServices();

        var handler = new TestCommandHandler(repository, unitOfWork, services);

        var result = await handler.Handle(new FakeCommand(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        handler.ExposedRepository.Should().BeSameAs(repository);
        handler.ExposedUnitOfWork.Should().BeSameAs(unitOfWork);
    }

    [Fact]
    public async Task Generic_handler_should_expose_repository_and_unit_of_work()
    {
        var repository = new FakeWriteRepository();
        var unitOfWork = new FakeUnitOfWork();
        var services = CreateServices();

        var handler = new TestCommandHandlerWithResponse(repository, unitOfWork, services);

        var result = await handler.Handle(new FakeCommandWithResponse(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        handler.ExposedRepository.Should().BeSameAs(repository);
        handler.ExposedUnitOfWork.Should().BeSameAs(unitOfWork);
    }

    [Fact]
    public void Non_generic_handler_ctor_should_throw_when_repository_is_null()
    {
        var unitOfWork = new FakeUnitOfWork();
        var services = CreateServices();

        var action = () => new TestCommandHandler(null!, unitOfWork, services);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Non_generic_handler_ctor_should_throw_when_unit_of_work_is_null()
    {
        var repository = new FakeWriteRepository();
        var services = CreateServices();

        var action = () => new TestCommandHandler(repository, null!, services);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Generic_handler_ctor_should_throw_when_repository_is_null()
    {
        var unitOfWork = new FakeUnitOfWork();
        var services = CreateServices();

        var action = () => new TestCommandHandlerWithResponse(null!, unitOfWork, services);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Generic_handler_ctor_should_throw_when_unit_of_work_is_null()
    {
        var repository = new FakeWriteRepository();
        var services = CreateServices();

        var action = () => new TestCommandHandlerWithResponse(repository, null!, services);

        action.Should().Throw<ArgumentNullException>();
    }

    private static HandlerServices CreateServices()
    {
        return new HandlerServices(
            new FakeMapperAdapter(),
            new FakeSerializer(),
            new FakeTranslator(),
            NullLoggerFactory.Instance,
            new FakeCurrentUser());
    }

    private sealed class TestCommandHandler
        : CommandHandlerBase<FakeCommand, FakeWriteRepository, FakeAggregateRoot, int>
    {
        public TestCommandHandler(
            FakeWriteRepository repository,
            FakeUnitOfWork unitOfWork,
            HandlerServices services)
            : base(repository, unitOfWork, services)
        {
        }

        public FakeWriteRepository ExposedRepository => Repository;

        public FakeUnitOfWork ExposedUnitOfWork => (FakeUnitOfWork)UnitOfWork;

        public override Task<Result> Handle(FakeCommand command, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success());
        }
    }

    private sealed class TestCommandHandlerWithResponse
        : CommandHandlerBase<FakeCommandWithResponse, string, FakeWriteRepository, FakeAggregateRoot, int>
    {
        public TestCommandHandlerWithResponse(
            FakeWriteRepository repository,
            FakeUnitOfWork unitOfWork,
            HandlerServices services)
            : base(repository, unitOfWork, services)
        {
        }

        public FakeWriteRepository ExposedRepository => Repository;

        public FakeUnitOfWork ExposedUnitOfWork => (FakeUnitOfWork)UnitOfWork;

        public override Task<Result<string>> Handle(
            FakeCommandWithResponse command,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success("ok"));
        }
    }
}