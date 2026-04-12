using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.Data.Abstractions.Services;
using ZaminX.Samples.Data.SqlServer.Endpoints;
using ZaminX.Samples.Data.SqlServer.Infrastructure.Auditing;
using ZaminX.Samples.Data.SqlServer.Infrastructure.Read;
using ZaminX.Samples.Data.SqlServer.Infrastructure.Write;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=.;Database=AxiomSampleSqlServer;Trusted_Connection=True;TrustServerCertificate=True;";

builder.Services.AddSingleton<IDataAuditContext, SampleDataAuditContext>();

builder.Services.AddZaminXDataAccess(dataAccess =>
{
    dataAccess.UseEntityFrameworkCore<AppWriteDbContext>(ef =>
    {
        ef.WithSqlServer(connectionString);
        ef.EnableAuditing();
        ef.AddWriteUnitOfWork();
        ef.ScanWriteRepositoriesFromAssemblies(typeof(OrderWriteRepository).Assembly);
    });

    dataAccess.UseReadEntityFrameworkCore<AppReadDbContext>(ef =>
    {
        ef.WithSqlServer(connectionString);
        ef.ScanReadRepositoriesFromAssemblies(typeof(OrderReadRepository).Assembly);
    });
});

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var writeDbContext = scope.ServiceProvider.GetRequiredService<AppWriteDbContext>();
    await writeDbContext.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapOrderEndpoints();

app.Run();