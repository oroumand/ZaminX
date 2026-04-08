using System.Security.Claims;
using Serilog.Core;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUserInfoAccessor, FakeUserInfoAccessor>();

builder.AddZaminXLogging(logging =>
{
    logging.UseConsole();

    logging.UseFile(options =>
    {
        options.Path = "logs/sample-.txt";
        options.Shared = true;
    });

    logging.UseSeq("http://localhost:5341");

    logging.WithMachineName();
    logging.WithEnvironmentName();
    logging.WithThreadId();
    logging.WithCorrelationId(options =>
    {
        options.HeaderName = "X-Correlation-ID";
        options.PropertyName = "CorrelationId";
    });
    logging.WithTraceAndSpan();
    logging.WithApplicationMetadata(options =>
    {
        options.ApplicationName = "ZaminX Logging Sample";
        options.ServiceName = "Logging.WebApiSample";
        options.ServiceVersion = "1.0.0";
    });
    logging.WithEnricher<SampleTagEnricher>();
    logging.UseRequestLogging(options => options.IncludeQueryString = true);
});

var app = builder.Build();

app.UseZaminXLogging();
app.UseZaminXLoggingContext(context =>
{
    context.SetUserIdFromClaims("sub", ClaimTypes.NameIdentifier);
    context.SetUserNameFromClaims("preferred_username", ClaimTypes.Name, "name");
    context.Set("TenantId", httpContext => httpContext.Request.Headers["X-Tenant-ID"].FirstOrDefault());
    context.Set("ImpersonatorUserId", (httpContext, services) => services.GetRequiredService<IUserInfoAccessor>().GetImpersonatorUserId(httpContext));
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation("Root endpoint called.");
    return Results.Ok(new { Message = "Hello from Zamin X Logging sample." });
});

app.MapGet("/claims", (HttpContext httpContext) =>
{
    if (httpContext.User.Identity?.IsAuthenticated is not true)
    {
        var identity = new ClaimsIdentity(
        [
            new Claim("sub", "42"),
            new Claim(ClaimTypes.Name, "demo-user"),
            new Claim("preferred_username", "demo-user")
        ], "sample");
        httpContext.User = new ClaimsPrincipal(identity);
    }

    return Results.Ok(httpContext.User.Claims.Select(c => new { c.Type, c.Value }));
});

app.MapGet("/headers", (HttpContext httpContext) =>
    Results.Ok(httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString())));

await app.RunWithZaminXLoggingAsync();

internal interface IUserInfoAccessor
{
    string? GetImpersonatorUserId(HttpContext httpContext);
}

internal sealed class FakeUserInfoAccessor : IUserInfoAccessor
{
    public string? GetImpersonatorUserId(HttpContext httpContext)
        => httpContext.Request.Headers["X-Impersonator-UserId"].FirstOrDefault();
}

internal sealed class SampleTagEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SampleTag", "WebApiSample"));
    }
}
