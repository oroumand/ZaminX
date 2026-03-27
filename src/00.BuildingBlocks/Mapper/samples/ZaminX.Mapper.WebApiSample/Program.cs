using Scalar.AspNetCore;
using ZaminX.Mapper.AutoMapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddZaminXAutoMapperAdapter();

var app = builder.Build();

app.MapScalarApiReference();


app.Run();
