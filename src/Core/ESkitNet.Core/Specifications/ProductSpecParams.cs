namespace ESkitNet.Core.Specifications;
public class ProductSpecParams
{
    public ProductSpecParams(string? brands = "", string? types = "", string? sort = "")
    {
        _brands = string.IsNullOrEmpty(brands) ? [] : [.. brands.Split(',', StringSplitOptions.RemoveEmptyEntries)];
        _types = string.IsNullOrEmpty(types) ? [] : [.. types.Split(',', StringSplitOptions.RemoveEmptyEntries)];
        Sort = sort ?? "";
    }
    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        //set => _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
        //set => _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
    }

    public string Sort { get; set; }
}
