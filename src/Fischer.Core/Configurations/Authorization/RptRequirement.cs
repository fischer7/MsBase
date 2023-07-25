using Microsoft.AspNetCore.Authorization;

namespace Fischer.Core.Configurations.Authorization;
public sealed class RptRequirement : IAuthorizationRequirement
{
    public string Resource { get; }
    public string Scope { get; }

    public RptRequirement(string resource, string scope)
    {
        Resource = resource;
        Scope = scope;
    }
}