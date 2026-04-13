namespace ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    string? FirstName { get; }
    string? LastName { get; }
    bool IsAuthenticated { get; }

    string? GetClaim(string claimType);
    IReadOnlyCollection<string> GetClaims(string claimType);
}
