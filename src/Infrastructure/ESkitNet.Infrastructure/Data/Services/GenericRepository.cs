using ESkitNet.Core.Interfaces;
using ESkitNet.Core.Specifications;

namespace ESkitNet.Infrastructure.Data.Services;

public class GenericRepository<TEntity, TKey>(StoreDbContext dbContext) : IGenericRepository<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : class
{
    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TKey>.GetQuery(dbContext.Set<TEntity>().AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> spec)
    {
        return SpecificationEvaluator<TEntity, TKey>.GetQuery<TEntity, TResult>(dbContext.Set<TEntity>().AsQueryable(), spec);
    }

    public void Add(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }

    public void Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public bool Exists(TKey id)
    {
        return dbContext.Set<TEntity>().Any(x => x.Id == id);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<TEntity>()
            .FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);

        //dbContext.Set<TEntity>().Attach(entity);
        //dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<TEntity?> GetOneWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<TResult?> GetOneWithSpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TResult>> GetAllWithSpecAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<long> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();

        query = specification.ApplyCriteria(query);

        return await query.LongCountAsync(cancellationToken);
    }
}
