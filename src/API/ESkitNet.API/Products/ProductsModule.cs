namespace ESkitNet.API.Products;

public class ProductsModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/products")
            .WithTags("Products Module")
            .RequireCors("CorsPolicy");

        group.MapGet("/", Payments.GetMethods.Endpoint.Handle)
            .WithName("GetProducts")
            .Produces<PaginatedResult<ProductDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        //.RequireCors("CorsPolicy");

        group.MapGet("/{id}", GetById.Endpoint.Handle)
            .WithName("GetProduct")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Product")
            .WithDescription("Get Product");

        group.MapPost("/", Create.Endpoint.Handle)
            .WithName("CreateProduct")
            .Produces<Create.Endpoint.Response>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");

        group.MapPut("/{id}", Update.Endpoint.Handle)
            .WithName("UpdateProduct")
            .Produces<Update.Endpoint.Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product");

        group.MapDelete("/{id}", Delete.Endpoint.Handle)
            .WithName("DeleteProduct")
            .Produces<Delete.Endpoint.Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product");
    }
}
