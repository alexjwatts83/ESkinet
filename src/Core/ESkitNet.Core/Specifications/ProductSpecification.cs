﻿namespace ESkitNet.Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParams) : base(x =>
        (string.IsNullOrWhiteSpace(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
        (!specParams.Brands.Any() || specParams.Brands.Contains(x.Brand)) &&
        (!specParams.Types.Any() || specParams.Types.Contains(x.Type))
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification(string? sort)
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
        ApplyDistinctSort(sort);
    }
}

public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification(string? sort)
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
        ApplyDistinctSort(sort);
    }
}