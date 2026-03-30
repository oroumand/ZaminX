using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions.Contracts;
using ZaminX.Mapper.WebApiSample.Models;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();
        builder.Services.AddZaminXAutoMapperAdapter(options =>
        {
            options.Assemblies.Add(typeof(Program).Assembly);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapGet("/map", (IMapperAdapter mapper) =>
        {
            var user = new UserEntity
            {
                Id = 1,
                FullName = "Alireza Oroumand"
            };

            var dto = mapper.Map<UserEntity, UserDto>(user);

            return Results.Ok(dto);
        });

        app.Run();
    }
}