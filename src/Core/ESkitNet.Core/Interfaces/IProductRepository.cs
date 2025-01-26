//using ESkitNet.Core.Pagination;

//namespace ESkitNet.Core.Interfaces;

//public interface IProductRepository
//{
//    Task<(int PageNumber, int PageSize, long Count, IEnumerable<Product> Data)> 
//        GetAsync(ProductsPaginationRequest request, CancellationToken cancellationToken);

//    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

//    Task<IReadOnlyList<string>> GetBrandsAsync(CancellationToken cancellationToken);

//    Task<IReadOnlyList<string>> GetTypesAsync(CancellationToken cancellationToken);

//    void Add(Product product);

//    void Update(Product product);

//    void Delete(Product product);

//    bool Exists(Guid id);

//    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
//}
