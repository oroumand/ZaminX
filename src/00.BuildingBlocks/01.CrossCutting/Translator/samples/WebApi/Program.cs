using Scalar.AspNetCore;
using ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddParrot(parrot =>
{
    parrot.UseSqlServer(options =>
    {
        builder.Configuration
            .GetSection("Parrot:SqlServer")
            .Bind(options);

        if (options.SeedTranslations.Count == 0)
        {
            options.SeedTranslations =
            [
                new ParrotSqlServerSeedTranslation("Common.Hello", "en-US", "Hello"),
                new ParrotSqlServerSeedTranslation("Common.World", "en-US", "World"),
                new ParrotSqlServerSeedTranslation("Common.Hello", "fa-IR", "سلام"),
                new ParrotSqlServerSeedTranslation("Common.World", "fa-IR", "دنیا"),
                new ParrotSqlServerSeedTranslation("Common.Welcome", "en-US", "Welcome"),
                new ParrotSqlServerSeedTranslation("Common.Welcome", "fa-IR", "خوش آمدید"),
                new ParrotSqlServerSeedTranslation("User.DisplayName", "en-US", "John"),
                new ParrotSqlServerSeedTranslation("User.DisplayName", "fa-IR", "کاربر"),
                new ParrotSqlServerSeedTranslation("Message.Greeting", "en-US", "Hello {0}"),
                new ParrotSqlServerSeedTranslation("Message.Greeting", "fa-IR", "سلام {0}"),
                new ParrotSqlServerSeedTranslation("Message.DoubleGreeting", "en-US", "{0} - {1}"),
                new ParrotSqlServerSeedTranslation("Message.DoubleGreeting", "fa-IR", "{0} - {1}"),
                new ParrotSqlServerSeedTranslation("Message.Price", "en-US", "Amount: {0:N2} {1} | Time: {2:yyyy-MM-dd HH:mm:ss}"),
                new ParrotSqlServerSeedTranslation("Message.Price", "fa-IR", "مبلغ: {0:N2} {1} | زمان: {2:yyyy-MM-dd HH:mm:ss}")
            ];
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();