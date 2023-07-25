using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;

namespace Fischer.Auth.Services;
public sealed class AuthCacheService : IAuthCacheService
{
    private const string Blacklist = "blacklist";

    private readonly IDistributedCache _distributedCache;

    public AuthCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<bool> IsTokenBlackListed(string token)
    {
        var key = $"{Blacklist}{token}";
        var blacklistPossuiToken = await _distributedCache.GetStringAsync(key) is not null;
        return blacklistPossuiToken;
    }

    public async Task AddTokenIntoBlackList(string token, JwtSecurityToken tokenJwt)
    {
        var distributedCacheOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = tokenJwt.ValidTo
        };

        var key = $"{Blacklist}{token}";
        await _distributedCache.SetStringAsync(key, "true", distributedCacheOptions);
    }

}