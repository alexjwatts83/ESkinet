﻿using System.Linq.Expressions;

namespace ESkitNet.Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    protected BaseSpecification() : this(null) { }
    public Expression<Func<T, bool>>? Criteria { get; } = criteria;

    public List<Expression<Func<T, object>>> Includes { get; } = [];

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public bool? IsDistinct { get; private set; }

    public string? DistinctSort { get; private set; }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    public List<string> ThenIncludeStrings { get; } = [];

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string thenIncludes)
    {
        ThenIncludeStrings.Add(thenIncludes);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }

    protected void ApplyDistinctSort(string? sort)
    {
        DistinctSort = sort;
    }

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        if (Criteria == null)
            return query;

        query = query.Where(Criteria);

        return query;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria)
    : BaseSpecification<T>(criteria),
    ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null) { }

    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>>? selectExpression)
    {
        Select = selectExpression;
    }
}