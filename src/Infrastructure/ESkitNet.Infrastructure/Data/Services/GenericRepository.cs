﻿using ESkitNet.Core.Interfaces;
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

    public async Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        //dbContext.Set<TEntity>().Attach(entity);
        //dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<TEntity?> GetByIdWithSpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAllWithSpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }
}
