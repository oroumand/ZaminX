using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZaminX.ApplicationPatterns.HandlerExecution;
using ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests;

public class ApplicationHandlerBaseTests
{
    [Fact]
    public void Dependencies_should_be_exposed_from_services()
    {
        var mapper = new FakeMapperAdapter();
        var serializer = new FakeSerializer();
        var translator = new FakeTranslator();
        var currentUser = new FakeCurrentUser();
        var loggerFactory = NullLoggerFactory.Instance;

        var services = new HandlerServices(
            mapper,
            serializer,
            translator,
            loggerFactory,
            currentUser);

        var handler = new TestApplicationHandler(services);

        handler.ExposedMapper.Should().BeSameAs(mapper);
        handler.ExposedSerializer.Should().BeSameAs(serializer);
        handler.ExposedTranslator.Should().BeSameAs(translator);
        handler.ExposedCurrentUser.Should().BeSameAs(currentUser);
    }

    [Fact]
    public void ResultContext_should_not_be_created_before_first_use()
    {
        var handler = CreateHandler();

        handler.IsResultContextCreated().Should().BeFalse();
    }

    [Fact]
    public void ResultContext_should_be_created_on_first_access()
    {
        var handler = CreateHandler();

        _ = handler.ExposedResultContext;

        handler.IsResultContextCreated().Should().BeTrue();
    }

    [Fact]
    public void Success_should_return_success_result()
    {
        var handler = CreateHandler();

        var result = handler.CallSuccess();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void Success_of_t_should_return_success_result_with_value()
    {
        var handler = CreateHandler();

        var result = handler.CallSuccess(123);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(123);
    }

    [Fact]
    public void Error_should_return_failure_result()
    {
        var handler = CreateHandler();

        var result = handler.CallError("sample.error", "Sample error");

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("sample.error");
        result.Errors.Single().Message.Should().Be("Sample error");
    }

    [Fact]
    public void Invalid_should_return_failure_result()
    {
        var handler = CreateHandler();

        var result = handler.CallInvalid("validation.error", "Validation failed");

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("validation.error");
    }

    [Fact]
    public void FailureFromContext_should_return_success_when_context_has_no_errors()
    {
        var handler = CreateHandler();

        var result = handler.CallFailureFromContext();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
    }

    [Fact]
    public void FailureFromContext_should_return_failure_when_context_has_errors()
    {
        var handler = CreateHandler();
        handler.ExposedResultContext.AddError("sample.error", "Sample error");

        var result = handler.CallFailureFromContext();

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("sample.error");
    }

    [Fact]
    public void FailureFromContext_of_t_should_return_success_with_value_when_context_has_no_errors()
    {
        var handler = CreateHandler();

        var result = handler.CallFailureFromContext(456);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(456);
    }

    [Fact]
    public void FailureFromContext_of_t_should_return_failure_when_context_has_errors()
    {
        var handler = CreateHandler();
        handler.ExposedResultContext.AddError("sample.error", "Sample error");

        var result = handler.CallFailureFromContext(456);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("sample.error");
    }

    private static TestApplicationHandler CreateHandler()
    {
        var services = new HandlerServices(
            new FakeMapperAdapter(),
            new FakeSerializer(),
            new FakeTranslator(),
            NullLoggerFactory.Instance,
            new FakeCurrentUser());

        return new TestApplicationHandler(services);
    }

    private sealed class TestApplicationHandler : ApplicationHandlerBase
    {
        public TestApplicationHandler(HandlerServices services) : base(services)
        {
        }

        public object ExposedMapper => Mapper;

        public object ExposedSerializer => Serializer;

        public object ExposedTranslator => Translator;

        public object? ExposedCurrentUser => CurrentUser;

        public ResultContext ExposedResultContext => ResultContext;

        public bool IsResultContextCreated() => HasResultContext();

        public Result CallSuccess() => Success();

        public Result<int> CallSuccess(int value) => Success(value);

        public Result CallError(string code, string message) => Error(code, message);

        public Result CallInvalid(string code, string message) => Invalid(code, message);

        public Result CallFailureFromContext() => FailureFromContext();

        public Result<int> CallFailureFromContext(int value) => FailureFromContext(value);

        private bool HasResultContext() => typeof(ApplicationHandlerBase)
            .GetField("_resultContext", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .GetValue(this) is not null;
    }
}