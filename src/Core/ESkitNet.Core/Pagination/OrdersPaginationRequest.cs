namespace ESkitNet.Core.Pagination;

public record OrdersPaginationRequest(string? Status = "", string? Email = "", int PageNumber = 1, int PageSize = 10) 
    : PaginationRequest(PageNumber, PageSize);
