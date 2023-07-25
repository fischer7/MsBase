using Fischer.Core.Domain.Enumerators;
using Microsoft.AspNetCore.Mvc;

namespace Fischer.Core.Application.Filters;
public class AuthorizationFilterAttribute : TypeFilterAttribute
{
    public AuthorizationFilterAttribute(RolePermissionEnum roleName, PermissionActionEnum claimValue)
        : base(typeof(AuthorizationFilter))
    {
        Arguments = new object[] { roleName, claimValue };
    }
}