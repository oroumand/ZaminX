using System.Security.Claims;

namespace ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.Configurations;

public sealed class PersonaAspNetCoreOptions
{
    public string? DefaultUserId { get; set; }
    public string? DefaultUserName { get; set; }
    public string? DefaultFirstName { get; set; }
    public string? DefaultLastName { get; set; }
    public string? DefaultIpAddress { get; set; }
    public string? DefaultUserAgent { get; set; }

    public string UserIdClaimType { get; set; } = ClaimTypes.NameIdentifier;
    public string UserNameClaimType { get; set; } = ClaimTypes.Name;
    public string FirstNameClaimType { get; set; } = ClaimTypes.GivenName;
    public string LastNameClaimType { get; set; } = ClaimTypes.Surname;
}