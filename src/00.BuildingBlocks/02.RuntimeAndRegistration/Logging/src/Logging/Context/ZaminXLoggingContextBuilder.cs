using System.Security.Claims;

namespace ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.Context;

public sealed class ZaminXLoggingContextBuilder
{
    private readonly List<ContextPropertyRegistration> _registrations = [];

    public IReadOnlyCollection<ContextPropertyRegistration> Registrations => _registrations;

    public ZaminXLoggingContextBuilder Set(string propertyName, Func<HttpContext, object?> valueFactory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(valueFactory);
        _registrations.Add(new ContextPropertyRegistration(propertyName, (httpContext, _) => valueFactory(httpContext)));
        return this;
    }

    public ZaminXLoggingContextBuilder Set(string propertyName, Func<HttpContext, IServiceProvider, object?> valueFactory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(valueFactory);
        _registrations.Add(new ContextPropertyRegistration(propertyName, valueFactory));
        return this;
    }

    public ZaminXLoggingContextBuilder SetUserId(Func<HttpContext, object?> valueFactory) => Set("UserId", valueFactory);
    public ZaminXLoggingContextBuilder SetUserId(Func<HttpContext, IServiceProvider, object?> valueFactory) => Set("UserId", valueFactory);
    public ZaminXLoggingContextBuilder SetUserName(Func<HttpContext, object?> valueFactory) => Set("UserName", valueFactory);
    public ZaminXLoggingContextBuilder SetUserName(Func<HttpContext, IServiceProvider, object?> valueFactory) => Set("UserName", valueFactory);

    public ZaminXLoggingContextBuilder SetUserIdFromClaim(string claimType) => SetUserIdFromClaims(claimType);
    public ZaminXLoggingContextBuilder SetUserNameFromClaim(string claimType) => SetUserNameFromClaims(claimType);

    public ZaminXLoggingContextBuilder SetUserIdFromClaims(params string[] claimTypes)
        => SetFromClaims("UserId", claimTypes);

    public ZaminXLoggingContextBuilder SetUserNameFromClaims(params string[] claimTypes)
        => SetFromClaims("UserName", claimTypes);

    private ZaminXLoggingContextBuilder SetFromClaims(string propertyName, params string[] claimTypes)
    {
        ArgumentNullException.ThrowIfNull(claimTypes);
        if (claimTypes.Length == 0)
        {
            throw new ArgumentException("At least one claim type must be provided.", nameof(claimTypes));
        }

        return Set(propertyName, httpContext =>
        {
            foreach (var claimType in claimTypes.Where(static x => string.IsNullOrWhiteSpace(x) is false))
            {
                var value = httpContext.User.FindFirst(claimType)?.Value;
                if (string.IsNullOrWhiteSpace(value) is false)
                {
                    return value;
                }
            }

            return null;
        });
    }
}
