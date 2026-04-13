using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Scalar.AspNetCore;
using System.Security.Claims;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddAuthorization();

builder.Services.AddPersonaAspNetCore(options =>
{
    options.DefaultUserId = "system";
    options.DefaultUserName = "anonymous";
    options.DefaultFirstName = "Anonymous";
    options.DefaultLastName = "User";
    options.DefaultIpAddress = "0.0.0.0";
    options.DefaultUserAgent = "Unknown";
});

var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", (ICurrentUser currentUser, IWebCurrentUser webCurrentUser) =>
{
    return Results.Ok(new
    {
        currentUser.IsAuthenticated,
        currentUser.UserId,
        currentUser.UserName,
        currentUser.FirstName,
        currentUser.LastName,
        webCurrentUser.IpAddress,
        webCurrentUser.UserAgent
    });
});

app.MapGet("/claims/{claimType}", (string claimType, ICurrentUser currentUser) =>
{
    return Results.Ok(new
    {
        ClaimType = claimType,
        Value = currentUser.GetClaim(claimType),
        Values = currentUser.GetClaims(claimType)
    });
});

app.MapGet("/login", async (HttpContext httpContext) =>
{
    var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, "1001"),
        new(ClaimTypes.Name, "jdoe"),
        new(ClaimTypes.GivenName, "John"),
        new(ClaimTypes.Surname, "Doe"),
        new("department", "Engineering"),
        new("department", "Platform")
    };

    var identity = new ClaimsIdentity(
        claims,
        CookieAuthenticationDefaults.AuthenticationScheme);

    var principal = new ClaimsPrincipal(identity);

    await httpContext.SignInAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        principal);

    return Results.Ok(new
    {
        Message = "Signed in successfully."
    });
});

app.MapGet("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Results.Ok(new
    {
        Message = "Signed out successfully."
    });
});

app.Run();