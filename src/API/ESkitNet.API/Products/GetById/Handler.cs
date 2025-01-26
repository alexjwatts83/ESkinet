namespace ESkitNet.API.Products.GetById;

public record Query(Guid Id) : IQuery<Result>;

public record Result(ProductDto Product);

public class Handler(IGenericRepository<Product, ProductId> repo) : IQueryHandler<Query, Result>
{
    public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
    {
        var productId = ProductId.Of(query.Id);
        var product = await repo.GetByIdAsync(productId, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        var dto = new ProductDto(product.Id.Value, product.Name, product.Description, product.Price, product.PictureUrl, product.Type, product.Brand, product.QuantityInStock);

        return new Result(dto);
    }
}