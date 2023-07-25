using Fischer.Core.Domain.Primitives;
using Fischer.Core.Persistence.Context;

namespace Fischer.Core.Persistence.Repository;
public interface IRepository<T> : IDisposable where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}