using Fischer.Auth.Domain.Entities;
using Fischer.Core.Configurations;
using Fischer.Core.Domain.Enumerators;
using Fischer.Core.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fischer.Auth.Persistence.Context;
public sealed class SeedAuthDbContext
{
    private readonly AuthDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppsettingsGet _appSettings;

    public SeedAuthDbContext(AuthDbContext dbContext,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppsettingsGet appSettings)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _appSettings = appSettings;
    }

    public async Task Seed()
    {
        if (await _dbContext.Users.AnyAsync())
        {
            return;
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await SeedData();
            await transaction.CommitAsync();
        }

        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task SeedData()
    {
        await SeedAdmin();
        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedAdmin()
    {
        var usernameAdmin = _appSettings["AdminSeed:UserName"] ?? string.Empty;
        var nomeAdmin = _appSettings["AdminSeed:Name"] ?? string.Empty;
        var emailAdmin = _appSettings["AdminSeed:Email"] ?? string.Empty;
        var passwordAdmin = _appSettings["AdminSeed:Password"] ?? string.Empty;

        var admin = new AppUser(nomeAdmin)
        {
            Id = Guid.NewGuid(),
            UserName = usernameAdmin,
            NormalizedUserName = usernameAdmin.ToUpper(),
            Email = emailAdmin,
            NormalizedEmail = emailAdmin.ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        admin.SetAlteredPassword();

        var adminRole = new AppRole()
        {
            Name = RolePermissionEnum.Administrator.GetDescription(),
            NormalizedName = RolePermissionEnum.Administrator.GetDescription().ToUpper()
        };

        var claimUser = GetClaimsFullPermissions(RolePermissionEnum.User.GetDescription());
        var claimAudit = GetClaimsFullPermissions(RolePermissionEnum.Audit.GetDescription());

        var result = await _userManager.CreateAsync(admin, passwordAdmin);
        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(admin, false);
        else
            throw new Exception("fail to create admin");

        await _roleManager.CreateAsync(adminRole);

        await _userManager.AddToRoleAsync(admin, RolePermissionEnum.Administrator.GetDescription());

        await _roleManager.AddClaimAsync(adminRole, claimUser);

        await _roleManager.AddClaimAsync(adminRole, claimAudit);

    }

    private static Claim GetClaimsFullPermissions(string permissaoClaimName)
    {
        return new Claim(permissaoClaimName, string.Concat(PermissionActionEnum.Create.GetDescription(),
            PermissionActionEnum.Read.GetDescription(),
            PermissionActionEnum.Update.GetDescription(),
            PermissionActionEnum.Delete.GetDescription(),
            PermissionActionEnum.Special.GetDescription()));
    }
}