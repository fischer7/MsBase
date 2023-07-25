using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace Fischer.Core.Extensions;
public static class HttpContextExtensions
{
    public static string? GetLoggedUserId(this HttpContext httpContext)
    {
        var claim = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }

    public static T? GetService<T>(this HttpContext httpContext)
    {
        var service = (T?)httpContext.RequestServices.GetService(typeof(T));
        return service;
    }

    public static string? GetBearerToken(this HttpContext httpContext)
    {
        if (!(httpContext.User.Identity?.IsAuthenticated ?? false))
            return default;

        var token = httpContext.Request.Headers[HeaderNames.Authorization].ToString();
        token = token.Replace("Bearer", string.Empty).Trim();
        return token;
    }

    public static IEnumerable<Claim> GetClaimsFromToken(this HttpContext httpContext)
    {
        if (!(httpContext.User.Identity?.IsAuthenticated ?? false))
            return Enumerable.Empty<Claim>();

        return httpContext.User.Claims;
    }
}