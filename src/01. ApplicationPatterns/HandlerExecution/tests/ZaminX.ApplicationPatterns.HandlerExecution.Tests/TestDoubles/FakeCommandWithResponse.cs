using ZaminX.BuildingBlocks.Application.Commands;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed record FakeCommandWithResponse : ICommand<string>;