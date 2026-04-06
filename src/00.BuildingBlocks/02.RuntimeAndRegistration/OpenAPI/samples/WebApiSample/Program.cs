using ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    build: lumen =>
    {
        lumen.UseScalar();
        lumen.UseSwagger();
        lumen.UseRedoc();
    });

var app = builder.Build();

app.UseZaminXOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () => Results.Ok(new
{
    Message = "Lumen sample is running.",
    OpenApi = "/openapi/v1.json",
    Scalar = "/scalar",
    Swagger = "/swagger",
    Redoc = "/redoc"
}));

app.Run();

internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
