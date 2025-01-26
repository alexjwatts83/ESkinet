using FluentValidation;

namespace ESkitNet.API.Products.Create;

public static class Endpoint
{
    public record Request(ProductDto Product);
    public record Response(Guid Id);


    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        return Results.Created($"/api/products/{response.Id}", response);
    }

    public record Command(ProductDto Product) : ICommand<Result>;
    public record Result(Guid Id);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Product).NotNull().WithMessage("Product can not be null");
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Product.Name).MaximumLength(100).WithMessage("Name must be less than 100 characters");
            RuleFor(x => x.Product.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Product.Description).MaximumLength(1000).WithMessage("Description must be less than 1000 characters");
            RuleFor(x => x.Product.PictureUrl).NotEmpty().WithMessage("PictureUrl cannot be empty");
            RuleFor(x => x.Product.Type).NotEmpty().WithMessage("Type cannot be empty");
            RuleFor(x => x.Product.Brand).NotEmpty().WithMessage("Brand cannot be empty");
            RuleFor(x => x.Product.QuantityInStock).NotEmpty().WithMessage("QuantityInStock cannot be empty");
            RuleFor(x => x.Product.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("QuantityInStock must be greater than or equal to 0");
            RuleFor(x => x.Product.Price).NotEmpty().WithMessage("Price cannot be empty");
            RuleFor(x => x.Product.Price).GreaterThanOrEqualTo(0).WithMessage("QuantityInStock must be greater than or equal to 0");
        }
    }

    public class Handler(IGenericRepository<Product, ProductId> repo) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            // TODO use a mapper
            var product = new Product()
            {
                Id = ProductId.Of(Guid.NewGuid()),
                Name = command.Product.Name,
                Description = command.Product.Description,
                Price = command.Product.Price,
                PictureUrl = command.Product.PictureUrl,
                Type = command.Product.Type,
                Brand = command.Product.Brand,
                QuantityInStock = command.Product.QuantityInStock,
            };

            repo.Add(product);

            await repo.SaveAllAsync(cancellationToken);

            return new Result(product.Id.Value);
        }
    }
}
