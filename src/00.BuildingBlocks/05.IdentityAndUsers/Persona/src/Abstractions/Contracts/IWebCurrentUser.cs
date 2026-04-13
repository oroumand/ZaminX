namespace ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

public interface IWebCurrentUser : ICurrentUser
{
    string? IpAddress { get; }
    string? UserAgent { get; }
}