using Fischer.Auth.Domain.Entities;
using Fischer.Auth.Persistence.Context;
using Fischer.Auth.Persistence.Repositories.Interfaces;
using Fischer.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Auth.Persistence.Repositories;
public sealed class RoleRepository : IRoleRepository
{
    private readonly AuthDbContext _context;
    public RoleRepository(AuthDbContext context) => _context = context;
    public IUnitOfWork UnitOfWork => _context;
    public IQueryable<AppRole> GetAll() => _context.Set<AppRole>().AsNoTracking();
    public void Update(AppRole perfil) => _context.Entry(perfil).State = EntityState.Modified;
    public async Task<bool> RoleExists(Guid roleId)
        => await _context.Roles.AnyAsync(p => p.Id == roleId);
    public void RemoveRole(AppRole role)
    {
        foreach (var roleClaim in role.RoleClaims ?? Enumerable.Empty<AppRoleClaim>())
        {
            _context.Entry(roleClaim).State = EntityState.Deleted;
        }
    }
    public void Dispose()
        => _context?.Dispose();
}