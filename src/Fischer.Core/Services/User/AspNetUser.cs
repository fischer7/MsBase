using Fischer.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Fischer.Core.Services.User;
public sealed class AspNetUser : IAspNetUser
{
    private readonly IHttpContextAccessor _accessor;

    public AspNetUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string Name => _accessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;

    public Guid? GetUserId()
    {
        return IsAuthenticated() ? Guid.Parse(_accessor.HttpContext?.User.GetUserId() ?? string.Empty) : default;
    }

    public string GetUserEmail()
    {
        return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserEmail() ?? string.Empty : string.Empty;
    }

    public string GetUserToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserToken() ?? string.Empty : "";
    }

    public string GetUserRefreshToken()
    {
        return IsAuthenticated() ? _accessor.HttpContext?.User.GetUserRefreshToken() ?? string.Empty : "";
    }

    public bool IsAuthenticated()
    {
        return _accessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }

    public bool IsInRole(string role)
    {
        return _accessor.HttpContext?.User.IsInRole(role) ?? false;
    }

    public IEnumerable<Claim> GetClaims()
    {
        return _accessor.HttpContext?.User.Claims ?? Enumerable.Empty<Claim>();
    }

    public HttpContext? GetHttpContext()
    {
        return _accessor.HttpContext;
    }
}