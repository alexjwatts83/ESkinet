using System.Linq.Expressions;

namespace ESkitNet.Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? Criteria { get; } = criteria;

    //public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    //public Expression<Func<T, object>> OrderBy { get; private set; }

    //public Expression<Func<T, object>> OrderByDescending { get; private set; }

    //public int Take { get; private set; }

    //public int Skip { get; private set; }

    //public bool IsPagingEnabled { get; private set; }

    //protected void AddInclude(Expression<Func<T, object>> includeExpression)
    //{
    //    Includes.Add(includeExpression);
    //}

    //protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    //{
    //    OrderBy = orderByExpression;
    //}

    //protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    //{
    //    OrderByDescending = orderByDescExpression;
    //}

    //protected void ApplyPaging(int skip, int take)
    //{
    //    Skip = skip;
    //    Take = take;
    //    IsPagingEnabled = true;
    //}
}

public class ProductSpecification(string? brand, string? type) : BaseSpecification<Product>(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) && 
        (string.IsNullOrWhiteSpace(type) || x.Type == type)
    )
{
}