using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace Fischer.Core.Configurations.Authorization;
public sealed class RptRequirementHandler : AuthorizationHandler<RptRequirement>
{
    private readonly ILogger<RptRequirementHandler> _logger;

    public RptRequirementHandler(ILogger<RptRequirementHandler> logger)
    {
        _logger = logger;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RptRequirement requirement)
    {
        if (!context?.User?.Identity?.IsAuthenticated ?? false)
            return Task.CompletedTask;

        var authorizationClaim = context?.User?.FindFirstValue("authorization") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(authorizationClaim))
        {
            return Task.CompletedTask;
        }

        var json = JsonDocument.Parse(authorizationClaim);
        var permissions = json.RootElement.GetProperty("permissions");
        foreach (var permission in permissions.EnumerateArray())
        {
            if (permission.GetProperty("rsname").GetString() != requirement.Resource)
            {
                continue;
            }

            if (permission.GetProperty("scopes")
                .EnumerateArray()
                .Any(scope => scope.GetString() == requirement.Scope))
            {
                context?.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
        return Task.CompletedTask;
    }
}