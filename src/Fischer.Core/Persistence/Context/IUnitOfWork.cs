namespace Fischer.Core.Persistence.Context;
public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}