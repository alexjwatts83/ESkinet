namespace ESkitNet.Core.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    bool Exists(TKey id);
    Task<bool> SaveAllAsync(CancellationToken cancellationToken);
}
