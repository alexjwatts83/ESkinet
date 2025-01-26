namespace ESkitNet.Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) && 
        (string.IsNullOrWhiteSpace(type) || x.Type == type)
    )
    {
        switch(sort)
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