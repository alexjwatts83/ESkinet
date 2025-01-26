using ESkitNet.Core.Abstractions;
using ESkitNet.Core.Interfaces;
using ESkitNet.Core.Pagination;
using ESkitNet.Infrastructure.Data.Extensions;

namespace ESkitNet.Infrastructure.Data.Services;

public class ProductRepository(StoreDbContext dbContext) : IProductRepository
{
    public void Add(Product product)
    {
        dbContext.Products.Add(product);
    }

    public void Delete(Product product)
    {
        dbContext.Products.Remove(product);
    }

    public bool Exists(Guid id)
    {
        var productId = ProductId.Of(id);

        return dbContext.Products.Any(x => x.Id == productId);
    }

    public async Task<(int PageNumber, int PageSize, long Count, IEnumerable<Product> Data)>
        GetAsync(ProductsPaginationRequest request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber <= 0
            ? 1
            : request.PageNumber;
        var pageSize = request.PageSize;

        var query = dbContext.Products
            .WhereIf(!string.IsNullOrWhiteSpace(request.Brand), x => x.Brand.ToLower() == request.Brand!.ToLower())
            .WhereIf(!string.IsNullOrWhiteSpace(request.Type), x => x.Type.ToLower() == request.Type!.ToLower());

        var totalCount = await query.LongCountAsync(cancellationToken);

        query = request.Sort switch
        {
            "priceAsc" => query.OrderBy(o => o.Price),
            "priceDesc" => query.OrderByDescending(o => o.Price),
            _ => query.OrderBy(o => o.Name)
        };

        var products = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (pageNumber, pageSize, totalCount, products);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Products.Select(x => x.Brand).Distinct().OrderBy(x => x).ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var productId = ProductId.Of(id);

        var product = await dbContext.Products.FindAsync([productId], cancellationToken: cancellationToken);

        return product;
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Products.Select(x => x.Type).Distinct().OrderBy(x => x).ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Update(Product product)
    {
        dbContext.Products.Update(product);

        /* alt */
        //dbContext.Entry(product).State = EntityState.Modified;
    }
}
