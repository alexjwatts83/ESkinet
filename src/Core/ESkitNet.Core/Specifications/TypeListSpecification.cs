namespace ESkitNet.Core.Specifications;

public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification(string? sort)
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
        ApplyDistinctSort(sort);
    }
}