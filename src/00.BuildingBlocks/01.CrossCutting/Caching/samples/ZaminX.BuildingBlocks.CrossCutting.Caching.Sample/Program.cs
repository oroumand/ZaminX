using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions.Models;
using ZaminX.BuildingBlocks.CrossCutting.Caching.Sample.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
//builder.Services.AddZaminXCachingWithInMemory();

//builder.Services.AddZaminXCachingWithRedis(options =>
//{
//    options.Configuration = "localhost:6379";
//    options.InstanceName = "StashXSample:";
//});

builder.Services.AddZaminXCachingWithSqlServer(options =>
{
    options.ConnectionString = "Server=.;Database=StashXSample;Trusted_Connection=True;TrustServerCertificate=True;User ID=sa;Password=1qaz!";
    options.SchemaName = "dbo";
    options.TableName = "StashXCache";
    options.EnsureStorageOnStartup = true;
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.MapGet("/", () => Results.Ok(new
{
    capability = "Caching",
    product = "StashX",
    activeProvider = "InMemory"
}));

app.MapPost("/cache", async (SetCacheRequest request, IStashX stashX, CancellationToken cancellationToken) =>
{
    var options = new CacheEntryOptions
    {
        AbsoluteExpiration = request.AbsoluteExpirationSeconds.HasValue
            ? TimeSpan.FromSeconds(request.AbsoluteExpirationSeconds.Value)
            : null,
        SlidingExpiration = request.SlidingExpirationSeconds.HasValue
            ? TimeSpan.FromSeconds(request.SlidingExpirationSeconds.Value)
            : null
    };

    await stashX.SetAsync(
        request.Key,
        request.Value,
        options,
        cancellationToken);

    return Results.Ok(new
    {
        message = "Value cached successfully.",
        request.Key,
        request.Value,
        request.AbsoluteExpirationSeconds,
        request.SlidingExpirationSeconds
    });
});

app.MapGet("/cache/{key}", async (string key, IStashX stashX, CancellationToken cancellationToken) =>
{
    var value = await stashX.GetAsync<string>(key, cancellationToken);

    return value is null
        ? Results.NotFound(new
        {
            message = "Cache entry not found.",
            key
        })
        : Results.Ok(new
        {
            key,
            value
        });
});

app.MapDelete("/cache/{key}", async (string key, IStashX stashX, CancellationToken cancellationToken) =>
{
    await stashX.RemoveAsync(key, cancellationToken);

    return Results.Ok(new
    {
        message = "Cache entry removed successfully.",
        key
    });
});

app.MapPost("/cache/{key}/get-or-set", async (string key, IStashX stashX, CancellationToken cancellationToken) =>
{
    var value = await stashX.GetOrSetAsync(
        key,
        _ => Task.FromResult($"generated:{key}:{DateTimeOffset.UtcNow:O}"),
        new CacheEntryOptions
        {
            AbsoluteExpiration = TimeSpan.FromMinutes(5)
        },
        cancellationToken);

    return Results.Ok(new
    {
        key,
        value
    });
});

app.Run();