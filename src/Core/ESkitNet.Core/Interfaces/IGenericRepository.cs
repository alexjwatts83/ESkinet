using ESkitNet.Core.Specifications;

namespace ESkitNet.Core.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdWithSpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> ListAllWithSpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    bool Exists(TKey id);
    Task<bool> SaveAllAsync(CancellationToken cancellationToken);
}
