namespace ESkitNet.Core.Specifications;


public class PagingSpecParams(int pageNumber = 1, int pageSize = 5)
{
    private const int _maxPageSize = 50;

    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; private set; } = (pageSize > _maxPageSize) 
        ? _maxPageSize 
        : pageSize;
}

public class OrdersSpecParams(
    string? email = "",
    string? status = "",
    int pageNumber = 1,
    int pageSize = 5) : PagingSpecParams(pageNumber, pageSize)
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

public class ProductSpecParams(
    string? brands = "",
    string? types = "",
    string? sort = "",
    string? search = "",
    int pageNumber = 1,
    int pageSize = 5) : PagingSpecParams(pageNumber, pageSize)
{
    public List<string> Brands { get; private set; } = string.IsNullOrWhiteSpace(brands) 
        ? [] 
        : [.. brands.Split(',', StringSplitOptions.RemoveEmptyEntries)];
    
    public List<string> Types { get; private set; } = string.IsNullOrWhiteSpace(types) 
        ? [] 
        : [.. types.Split(',', StringSplitOptions.RemoveEmptyEntries)];

    public string Sort { get; set; } = sort ?? "";

    public string Search { get; private set; } = string.IsNullOrWhiteSpace(search) 
        ? string.Empty 
        : search.ToLower();
}
