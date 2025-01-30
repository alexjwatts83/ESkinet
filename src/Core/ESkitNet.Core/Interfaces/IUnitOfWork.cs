namespace ESkitNet.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>()
        where TEntity : Entity<TKey>
        where TKey : class;

    Task<bool> Complete(CancellationToken cancellationToken = default);
}
