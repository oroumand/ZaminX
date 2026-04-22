using ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.Contracts;

namespace ZaminX.ApplicationPatterns.HandlerExecution.Tests.TestDoubles;

public sealed class FakeCurrentUser : ICurrentUser
{
    private readonly Dictionary<string, List<string>> _claims = new(StringComparer.OrdinalIgnoreCase);

    public string? UserId { get; init; } = "1";

    public string? UserName { get; init; } = "test-user";

    public string? FirstName { get; init; } = "Test";

    public string? LastName { get; init; } = "User";

    public bool IsAuthenticated { get; init; } = true;

    public string? GetClaim(string claimType)
    {
        return _claims.TryGetValue(claimType, out var values)
            ? values.FirstOrDefault()
            : null;
    }

    public IReadOnlyCollection<string> GetClaims(string claimType)
    {
        return _claims.TryGetValue(claimType, out var values)
            ? values.AsReadOnly()
            : Array.Empty<string>();
    }

    public FakeCurrentUser AddClaim(string claimType, params string[] values)
    {
        if (!_claims.TryGetValue(claimType, out var list))
        {
            list = [];
            _claims[claimType] = list;
        }

        list.AddRange(values);

        return this;
    }
}