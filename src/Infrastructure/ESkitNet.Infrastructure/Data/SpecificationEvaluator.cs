using ESkitNet.Core.Specifications;

namespace ESkitNet.Infrastructure.Data;
public static class SpecificationEvaluator<TEntity, TKey>
    where TEntity : Entity<TKey>
    where TKey : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        return query;
    }
}
