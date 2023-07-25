using System.IdentityModel.Tokens.Jwt;

namespace Fischer.Auth.Services;
public interface IAuthCacheService
{
    Task<bool> IsTokenBlackListed(string token);
    Task AddTokenIntoBlackList(string token, JwtSecurityToken tokenJwt);
}