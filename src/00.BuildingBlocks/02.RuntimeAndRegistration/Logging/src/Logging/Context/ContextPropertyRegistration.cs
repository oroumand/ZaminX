namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Context;

public sealed record ContextPropertyRegistration(
    string PropertyName,
    Func<HttpContext, IServiceProvider, object?> ValueFactory);
