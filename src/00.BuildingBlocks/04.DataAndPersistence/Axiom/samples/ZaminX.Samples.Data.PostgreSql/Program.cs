using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.Data.Abstractions.Services;
using ZaminX.Samples.Data.PostgreSql.Endpoints;
using ZaminX.Samples.Data.PostgreSql.Infrastructure.Auditing;
using ZaminX.Samples.Data.PostgreSql.Infrastructure.Read;
using ZaminX.Samples.Data.PostgreSql.Infrastructure.Write;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Port=5432;Database=axiom_sample_pg;Username=postgres;Password=postgres";

builder.Services.AddSingleton<IDataAuditContext, SampleDataAuditContext>();

builder.Services.AddZaminXDataAccess(dataAccess =>
{
    dataAccess.UseEntityFrameworkCore<AppWriteDbContext>(ef =>
    {
        ef.WithPostgreSql(connectionString);
        ef.EnableAuditing();
        ef.AddWriteUnitOfWork();
        ef.ScanWriteRepositoriesFromAssemblies(typeof(OrderWriteRepository).Assembly);
    });

    dataAccess.UseReadEntityFrameworkCore<AppReadDbContext>(ef =>
    {
        ef.WithPostgreSql(connectionString);
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