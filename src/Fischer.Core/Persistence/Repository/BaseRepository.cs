//http://www.linhadecodigo.com.br/artigo/3370/entity-framework-4-repositorio-generico.aspx
using Fischer.Core.Domain.Primitives;
using Fischer.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Fischer.Core.Persistence.Repository;
public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : Entity
{
    private readonly CoreDbContext _context;

    protected BaseRepository(CoreDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public IQueryable<TEntity> Get() => _context.Set<TEntity>().AsQueryable();

    public IQueryable<TEntity> AsNoTracking() => Get().AsNoTrackingWithIdentityResolution();

    public void Add(TEntity entity) => _context.Add(entity);

    public void Update(TEntity entity) => _context.Entry(entity).State = EntityState.Modified;

    public void Delete(TEntity entity) => _context.Remove(entity);

    public void Dispose()
    {
        _context?.Dispose();
    }
}