namespace ESkitNet.Core.Pagination;

public record ProductsPaginationRequest(string? Brand, string? Type) : PaginationRequest;
