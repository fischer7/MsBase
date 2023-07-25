using Fischer.Core.Domain.Enumerators;
using Fischer.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Security.Claims;

namespace Fischer.Core.Application.Filters;
public class AuthorizationFilter : IAuthorizationFilter
{
    private const string BLACKLIST = "blacklist";

    private readonly RolePermissionEnum _claimName;
    private readonly PermissionActionEnum _claimValue;

    public AuthorizationFilter(RolePermissionEnum claimName,
                               PermissionActionEnum claimValue)
    {
        _claimName = claimName;
        _claimValue = claimValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false
            || !ValidateAuth(context.HttpContext))
            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);

        if (!ValidateClaims(context.HttpContext.User, _claimName, _claimValue))
            context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
    }

    private bool ValidateAuth(HttpContext context)
        => context.User.Identity?.IsAuthenticated ?? false && IsValidToken(context);

    public static bool ValidateClaims(
            ClaimsPrincipal user,
            RolePermissionEnum claimName,
            PermissionActionEnum claimValue)
    {
        return user.IsInRole(RolePermissionEnum.Administrator.GetDescription())
            || user.Claims.Any(c => c.Type == claimName.GetDescription()
                && c.Value.Contains(claimValue.GetDescription()));
    }

    private static bool IsValidToken(HttpContext context)
    {
        var token = context.GetBearerToken();
        if (token is null)
            return false;

        var key = $"{BLACKLIST}{token}";

        var distributedCache = context.GetService<IDistributedCache>();
        var blackListPossuiToken = distributedCache?.GetString(key) is not null;

        return !blackListPossuiToken;
    }
}