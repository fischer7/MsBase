namespace Fischer.Core.Configurations.Authorization;

public sealed class JWTConfig
{
    public string? Secret { get; set; }
    public int ExpirationTime { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
}