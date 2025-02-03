namespace ESkitNet.Core.Pagination;

public record ProductsPaginationRequest(string? Brand, string? Type, string? Sort, string? Search, int PageNumber = 1, int PageSize = 10) 
    : PaginationRequest(PageNumber, PageSize);

public record OrdersPaginationRequest(string? Status, string? Email, int PageNumber = 1, int PageSize = 10) 
    : PaginationRequest(PageNumber, PageSize);
