//https://www.codeguru.com/csharp/repository-pattern-c-sharp/
using Fischer.Core.Domain.Primitives;
using Fischer.Core.Persistence.Context;

namespace Fischer.Core.Persistence.Repository;
public interface IBaseRepository<TEntity> where TEntity : Entity
{
    IUnitOfWork UnitOfWork { get; }
    void Add(TEntity entity);
    IQueryable<TEntity> Get();
    IQueryable<TEntity> AsNoTracking();
    void Update(TEntity entity);
    void Delete(TEntity entity);
}