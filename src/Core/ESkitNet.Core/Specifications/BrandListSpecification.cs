namespace ESkitNet.Core.Specifications;

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification(string? sort)
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
        ApplyDistinctSort(sort);
    }
}
