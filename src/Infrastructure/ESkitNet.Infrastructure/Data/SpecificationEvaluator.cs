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
            query = query.Where(specification.Criteria);

        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);

        if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        if (specification.IsDistinct.HasValue && specification.IsDistinct.Value)
            query = query.Distinct();

        //if (specification.IsDistinctOrdered.HasValue && specification.IsDistinctOrdered.Value)
        //    query = query.OrderBy(x => x);

        //if (specification.IsDistinctOrderedDesc.HasValue && specification.IsDistinctOrderedDesc.Value)
        //    query = query.OrderByDescending(x => x);

        switch (specification.DistinctSort)
        {
            case "asc":
                query = query.OrderBy(x => x);
                break;
            case "desc":
                query = query.OrderByDescending(x => x);
                break;
            default:
                break;
        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TResult> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);

        if (specification.OrderByDescending != null)
            query = query.OrderByDescending(specification.OrderByDescending);

        if (specification.Select == null)
            return query.Cast<TResult>();

        var selectQuery = query.Select(specification.Select);

        if (specification.IsDistinct.HasValue && specification.IsDistinct.Value)
            selectQuery = selectQuery?.Distinct();

        //if (specification.IsDistinctOrdered.HasValue && specification.IsDistinctOrdered.Value)
        //    selectQuery = selectQuery?.OrderBy(x => x);

        //if (specification.IsDistinctOrderedDesc.HasValue && specification.IsDistinctOrderedDesc.Value)
        //    selectQuery = selectQuery?.OrderByDescending(x => x);

        switch (specification.DistinctSort)
        {
            case "asc":
                selectQuery = selectQuery?.OrderBy(x => x);
                break;
            case "desc":
                selectQuery = selectQuery?.OrderByDescending(x => x);
                break;
            default:
                break;
        }

        return selectQuery!;
    }
}
