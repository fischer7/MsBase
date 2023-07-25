using Fischer.Auth.Domain.Entities;
using Fischer.Core.Persistence.Repository;

namespace Fischer.Auth.Persistence.Repositories.Interfaces;
public interface IUserRepository : IRepository<AppUser>
{
    Task<List<AppRoleClaim>> GetRoleClaims(Guid userId);
    Task<bool> UserExists(Guid userId);
    IQueryable<AppUser> GetAll();
}