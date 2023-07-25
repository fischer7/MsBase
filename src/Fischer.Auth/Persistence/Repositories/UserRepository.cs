using Fischer.Auth.Domain.Entities;
using Fischer.Auth.Persistence.Context;
using Fischer.Auth.Persistence.Repositories.Interfaces;
using Fischer.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Auth.Persistence.Repositories;
public sealed class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;

    public async Task<List<AppRoleClaim>> GetRoleClaims(Guid userId)
    {
        var userRoles = await _context.UserRoles
            .Include(up => up.RoleId)
            .Where(u => u.UserId == userId)
            .Select(up => up.RoleId).ToListAsync();

        var roles = _context.RoleClaims.Where(x => userRoles.Contains(x.RoleId));
        return roles.ToList();
    }

    public async Task<bool> UserExists(Guid userId)
        => await _context.Users.AnyAsync(u => u.Id == userId);

    public IQueryable<AppUser> GetAll() => _context.Users.AsNoTracking();

    public void Dispose() => _context.Dispose();
}