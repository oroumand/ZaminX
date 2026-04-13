using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Configurations;
using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Internals;

namespace ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Services;

public sealed class HttpContextCurrentUser : IWebCurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly PersonaAspNetCoreOptions _options;

    public HttpContextCurrentUser(
        IHttpContextAccessor httpContextAccessor,
        IOptions<PersonaAspNetCoreOptions> options)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public string? UserId => ResolveClaim(_options.UserIdClaimType, _options.DefaultUserId);

    public string? UserName => ResolveClaim(_options.UserNameClaimType, _options.DefaultUserName);

    public string? FirstName => ResolveClaim(_options.FirstNameClaimType, _options.DefaultFirstName);

    public string? LastName => ResolveClaim(_options.LastNameClaimType, _options.DefaultLastName);

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
        ?? _options.DefaultIpAddress;

    public string? UserAgent
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.Request?.Headers.UserAgent.ToString();
            return string.IsNullOrWhiteSpace(value) ? _options.DefaultUserAgent : value;
        }
    }

    public string? GetClaim(string claimType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(claimType);

        return _httpContextAccessor.HttpContext?.User.GetClaimValue(claimType);
    }

    public IReadOnlyCollection<string> GetClaims(string claimType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(claimType);

        return _httpContextAccessor.HttpContext?.User.GetClaimValues(claimType)
               ?? Array.Empty<string>();
    }

    private string? ResolveClaim(string claimType, string? fallback)
    {
        var value = _httpContextAccessor.HttpContext?.User.GetClaimValue(claimType);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}