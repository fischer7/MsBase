using Fischer.Auth.Domain.Entities;
using Fischer.Core.Application.Abstractions;
using Fischer.Core.Configurations;
using Fischer.Core.Configurations.Authorization;
using Fischer.Core.Constants;
using Fischer.Core.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fischer.Auth.Application.Queries.Login;

internal sealed class LoginUserQueryHandler : IQueryHandler<LoginUserQueryInput, LoginUserQueryResult>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly AppsettingsGet _appSettings;

    public LoginUserQueryHandler(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<AppRole> roleManager,
        AppsettingsGet appSettings)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _appSettings = appSettings;
    }
    public async Task<Result<LoginUserQueryResult>> Handle(LoginUserQueryInput request, CancellationToken cancellationToken)
    {
        var user = await GetUserByUsernameOrEmail(request.UserName);
        if (user is null)
            return new LoginUserQueryResult(false, string.Empty, ErrorConstants.UserOrPassInvalid);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, lockoutOnFailure: false);

        if (!result.Succeeded)
            return new LoginUserQueryResult(false, string.Empty, ErrorConstants.UserOrPassInvalid);

        var claims = (await _userManager.GetClaimsAsync(user)).ToList();
        var tokenJwt = await GenerateTokenWithClaims(claims, user);

        if (string.IsNullOrEmpty(tokenJwt))
            return new LoginUserQueryResult(false, string.Empty, ErrorConstants.InvalidLoginToken);

        return new LoginUserQueryResult(true, tokenJwt, ErrorConstants.None);
    }

    private async Task<string> GenerateTokenWithClaims(List<Claim> claims, AppUser user)
    {
        var claimsIdentity = await GetUserClaimsAsync(claims, user);
        var tokenJwt = GenerateToken(claimsIdentity);
        return tokenJwt;
    }

    private async Task<AppUser?> GetUserByUsernameOrEmail(string userName)
    {
        var user = await _userManager.FindByEmailAsync(userName);
        if (user is null)
            user = await _userManager.FindByNameAsync(userName);
        return user;
    }

    private async Task<ClaimsIdentity> GetUserClaimsAsync(List<Claim> claims, AppUser user)
    {
        claims.AddRange(new List<Claim>
                {
                    new (ClaimTypes.Name, user.UserName ?? string.Empty),
                    new (ClaimTypes.NameIdentifier, user.Id.ToString())
                });

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var roleName in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null)
                continue;
            var claimsInRole = await _roleManager.GetClaimsAsync(role);

            claims.AddRange(claimsInRole);
        }

        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaims(claims);
        return claimsIdentity;
    }


    private string GenerateToken(ClaimsIdentity identityClaims)
    {
        var jwtConfig = _appSettings.GetConfig<JWTConfig>();
        if (jwtConfig is null || string.IsNullOrWhiteSpace(jwtConfig.Secret))
            return string.Empty;

        var chave = Encoding.ASCII.GetBytes(jwtConfig.Secret);

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = jwtConfig.Issuer,
            Audience = jwtConfig.Audience,
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddHours(jwtConfig.ExpirationTime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
        });

        return tokenHandler.WriteToken(token);
    }
}
