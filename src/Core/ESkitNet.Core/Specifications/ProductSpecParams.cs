using ESkitNet.Core.Pagination;

namespace ESkitNet.Core.Specifications;


public class PagingSpecParams(PaginationRequest request)
{
    private const int _maxPageSize = 50;

    public int PageNumber { get; set; } = request.PageNumber;
    public int PageSize { get; private set; } = (request.PageSize > _maxPageSize)
        ? _maxPageSize
        : request.PageSize;
}

public class OrdersSpecParams()
{
    public OrdersSpecParams(string email) : this()
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email));
        }

        Email = email;
    }
    public string? Email { get; private set; }
}

public class OrdersPagingSpecParams(OrdersPaginationRequest request) : PagingSpecParams(request)
{
    public string? Status { get; private set; } = request.Status;
    public string? Email { get; private set; } = request.Email;
}

public class OrderSpecParams(OrderRequest request)
{
    public string? Email { get; private set; } = request.Email;
    public OrderId? Id { get; private set; } = request.Id.HasValue ? OrderId.Of(request.Id.Value) : null;
    public string? PaymentId { get; private set; } = request.PaymentId;
}

public class ProductSpecParams(ProductsPaginationRequest request) : PagingSpecParams(request)
{
    public List<string> Brands { get; private set; } = string.IsNullOrWhiteSpace(request.Brand)
        ? []
        : [.. request.Brand.Split(',', StringSplitOptions.RemoveEmptyEntries)];

    public List<string> Types { get; private set; } = string.IsNullOrWhiteSpace(request.Type)
        ? []
        : [.. request.Type.Split(',', StringSplitOptions.RemoveEmptyEntries)];

    public string Sort { get; set; } = request.Sort ?? "";

    public string Search { get; private set; } = string.IsNullOrWhiteSpace(request.Search)
        ? string.Empty
        : request.Search.ToLower();
}
