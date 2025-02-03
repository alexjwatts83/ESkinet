namespace ESkitNet.Core.Specifications;


public class PagingSpecParams
{
    public int PageNumber { get; set; } = 1;

    private const int _maxPageSize = 50;
    protected internal int _pageSize = 5;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }
}

public class OrdersSpecParams(string? email = "", string? status = "") : PagingSpecParams
{
    public string? Status { get; private set; } = status;
    public string? Email { get; private set; } = email;
}

public class OrderSpecParams(string? email = "", Guid? id = null, string? paymentId = "")
{
    public string? Email { get; private set; } = email;
    public OrderId? Id { get; private set; } = id.HasValue ? OrderId.Of(id.Value) : null;
    public string? PaymentId { get; private set; } = paymentId;
}

public class ProductSpecParams : PagingSpecParams
{
    // TODO I don't really like how this is done pretty it up later or find a better approach online
    public ProductSpecParams(string? brands = "", string? types = "", string? sort = "", string? search = "", int pageNumber = 1, int pageSize = 5)
    {
        _brands = string.IsNullOrEmpty(brands) ? [] : [.. brands.Split(',', StringSplitOptions.RemoveEmptyEntries)];
        _types = string.IsNullOrEmpty(types) ? [] : [.. types.Split(',', StringSplitOptions.RemoveEmptyEntries)];
        Sort = sort ?? "";
        PageNumber = pageNumber;
        _pageSize = pageSize;
        _search = search;
    }

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
    }

    private List<string> _types = [];
    public List<string> Types
    {
        get => _types;
    }

    public string Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}
