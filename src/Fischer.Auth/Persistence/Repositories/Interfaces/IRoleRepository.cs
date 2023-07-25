using Fischer.Auth.Domain.Entities;
using Fischer.Core.Persistence.Repository;

namespace Fischer.Auth.Persistence.Repositories.Interfaces;
public interface IRoleRepository : IRepository<AppRole>
{
    public IQueryable<AppRole> GetAll();
    public void Update(AppRole perfil);
    public void RemoveRole(AppRole perfil);
    Task<bool> RoleExists(Guid perfilId);
}

