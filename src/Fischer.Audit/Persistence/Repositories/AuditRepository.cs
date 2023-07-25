using Fischer.Audit.Persistence.Contexts;
using Fischer.Audit.Persistence.Repositories.Interfaces;
using Fischer.Core.Persistence.Repository;

namespace Fischer.Audit.Persistence.Repositories;
public class AuditRepository : BaseRepository<Domain.Audit>, IAuditRepository
{
    public AuditRepository(AuditDbContext context) : base(context) { }
}