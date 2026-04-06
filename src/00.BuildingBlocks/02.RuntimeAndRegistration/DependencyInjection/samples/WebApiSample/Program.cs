using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.Abstractions.Models;
using ZaminX.BuildingBlocks.RuntimeAndRegistration.DependencyInjection.WebApiSample.Services;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();

        builder.Services.AddZaminXDependencyInjection(options =>
        {
            options.AddAssemblyContaining<Program>();
            options.EnableConventionalRegistration(ServiceLifetime.Scoped);
            options.DuplicateRegistrationBehavior = DuplicateRegistrationBehavior.Skip;
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapGet("/", (IClock clock, IRequestIdGenerator requestIdGenerator) => Results.Ok(new
        {
            capability = "DependencyInjection",
            family = "RuntimeAndRegistration",
            markerBased = clock.UtcNow,
            conventionBased = requestIdGenerator.Create()
        }));

        app.Run();
    }
}