using ESkitNet.Core.Specifications;

namespace ESkitNet.Core.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : Entity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task<TEntity?> GetOneWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
    Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);

    Task<TResult?> GetOneWithSpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken);
    Task<IReadOnlyList<TResult>> GetAllWithSpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken);

    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    bool Exists(TKey id);
    Task<long> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken);
}
