using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests;

public class QueryHandlerBaseTests
{
    [Fact]
    public async Task Handler_should_expose_repository()
    {
        var repository = new FakeReadRepository();
        var services = CreateServices();

        var handler = new TestQueryHandler(repository, services);

        var result = await handler.Handle(new FakeQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("ok");
        handler.ExposedRepository.Should().BeSameAs(repository);
    }

    [Fact]
    public void Ctor_should_throw_when_repository_is_null()
    {
        var services = CreateServices();

        var action = () => new TestQueryHandler(null!, services);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Handler_should_be_able_to_use_repository()
    {
        var repository = new FakeReadRepository()
            .Seed(new FakeEntity(10));

        var services = CreateServices();
        var handler = new TestQueryHandler(repository, services);

        var result = await handler.Handle(new FakeQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("10");
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

    private sealed class TestQueryHandler
        : QueryHandlerBase<FakeQuery, string, FakeReadRepository, FakeEntity, int>
    {
        public TestQueryHandler(
            FakeReadRepository repository,
            HandlerServices services)
            : base(repository, services)
        {
        }

        public FakeReadRepository ExposedRepository => Repository;

        public override async Task<Result<string>> Handle(
            FakeQuery query,
            CancellationToken cancellationToken = default)
        {
            var entity = await Repository.GetByIdAsync(10, cancellationToken);

            return entity is null
                ? Success("ok")
                : Success(entity.Id.ToString());
        }
    }
}