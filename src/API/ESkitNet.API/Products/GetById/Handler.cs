namespace ESkitNet.API.Products.GetById;

public record Query(Guid Id) : IQuery<Result>;

public record Result(ProductDto Product);

public class Handler(StoreDbContext dbContext) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        // get orders with pagination
        // return result
        var productId = ProductId.Of(query.Id);
        var product = await dbContext.Products.FindAsync([productId], cancellationToken: cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        var dto = new ProductDto(product.Id.Value, product.Name, product.Description, product.Price, product.PictureUrl, product.Type, product.ProductBrand, product.QuantityInStock);

        return new Result(dto);
    }
}