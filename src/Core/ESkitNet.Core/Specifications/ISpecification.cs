﻿using System.Linq.Expressions;

namespace ESkitNet.Core.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    //List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    //int Take { get; }
    //int Skip { get; }
    //bool IsPagingEnabled { get; }
    bool? IsDistinct { get; }
    //bool? IsDistinctOrdered { get; }
    //bool? IsDistinctOrderedDesc { get; }
    string? DistinctSort { get; }
}

public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}