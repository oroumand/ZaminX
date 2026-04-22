using FluentAssertions;
using ZaminX.ApplicationPatterns.HandlerExecution;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests;

public class ResultContextTests
{
    [Fact]
    public void New_context_should_not_have_errors()
    {
        var context = new ResultContext();

        context.HasErrors.Should().BeFalse();
        context.Errors.Should().BeEmpty();
    }

    [Fact]
    public void AddError_should_add_error_to_context()
    {
        var context = new ResultContext();
        var error = new Error("sample.error", "Sample error");

        context.AddError(error);

        context.HasErrors.Should().BeTrue();
        context.Errors.Should().ContainSingle();
        context.Errors.Should().Contain(error);
    }

    [Fact]
    public void AddError_with_code_and_message_should_add_error_to_context()
    {
        var context = new ResultContext();

        context.AddError("sample.error", "Sample error");

        context.HasErrors.Should().BeTrue();
        context.Errors.Should().ContainSingle();

        var error = context.Errors.Single();
        error.Code.Should().Be("sample.error");
        error.Message.Should().Be("Sample error");
    }

    [Fact]
    public void AddErrors_should_add_all_errors_to_context()
    {
        var context = new ResultContext();
        var errors = new[]
        {
            new Error("error.1", "Error 1"),
            new Error("error.2", "Error 2")
        };

        context.AddErrors(errors);

        context.HasErrors.Should().BeTrue();
        context.Errors.Should().HaveCount(2);
        context.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void ToResult_should_return_success_when_context_has_no_errors()
    {
        var context = new ResultContext();

        var result = context.ToResult();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ToResult_should_return_failure_when_context_has_errors()
    {
        var context = new ResultContext();
        context.AddError("sample.error", "Sample error");

        var result = context.ToResult();

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("sample.error");
    }

    [Fact]
    public void ToResult_of_t_should_return_success_with_data_when_context_has_no_errors()
    {
        var context = new ResultContext();

        var result = context.ToResult(123);

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(123);
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ToResult_of_t_should_return_failure_when_context_has_errors()
    {
        var context = new ResultContext();
        context.AddError("sample.error", "Sample error");

        var result = context.ToResult(123);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.Single().Code.Should().Be("sample.error");
    }

    [Fact]
    public void AddError_should_throw_when_error_is_null()
    {
        var context = new ResultContext();

        var action = () => context.AddError((Error)null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddErrors_should_throw_when_errors_is_null()
    {
        var context = new ResultContext();

        var action = () => context.AddErrors(null!);

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddErrors_should_throw_when_collection_contains_null_item()
    {
        var context = new ResultContext();
        Error[] errors =
        [
            new Error("error.1", "Error 1"),
            null!
        ];

        var action = () => context.AddErrors(errors);

        action.Should().Throw<ArgumentNullException>();
    }
}