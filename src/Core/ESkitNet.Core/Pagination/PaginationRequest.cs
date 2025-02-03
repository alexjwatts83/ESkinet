namespace ESkitNet.Core.Pagination;

public abstract record PaginationRequest(int PageNumber = 1, int PageSize = 10);

public record OrderRequest(string? Email = "", Guid? Id = null, string? PaymentId = "");
