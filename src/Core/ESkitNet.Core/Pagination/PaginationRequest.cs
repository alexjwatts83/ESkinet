namespace ESkitNet.Core.Pagination;

public abstract record PaginationRequest(int PageNumber = 1, int PageSize = 10);
