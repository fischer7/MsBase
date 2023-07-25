namespace Fischer.Core.Configurations.Authorization;
public sealed class KeycloakConfig
{
    public string? ResourceUrl { get; set; }
    public string? JwtAuthority { get; set; }
    public string? JwtAudience { get; set; }
    public string? MetadataAddress { get; set; }
    public string? TokenUrl { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? GrantType { get; set; }
    public string? SecondGrantType { get; set; }
    public string? Scope { get; set; }
}