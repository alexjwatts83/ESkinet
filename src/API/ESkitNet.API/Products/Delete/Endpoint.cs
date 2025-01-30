namespace ESkitNet.API.Products.Delete;

public static class Endpoint
{
    public record Response(bool IsSuccess);
    public record Command(Guid ProductId) : ICommand<Result>;

    public record Result(bool IsSuccess);

    public class Handler(IUnitOfWork unitOfWork) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product, ProductId>().GetByIdAsync(ProductId.Of(command.ProductId), cancellationToken);

            if (product is null)
                throw new ProductNotFoundException(command.ProductId);

            unitOfWork.Repository<Product, ProductId>().Delete(product);

            var result = await unitOfWork.Complete(cancellationToken);

            return new Result(result);
        }
    }

    public static async Task<IResult> Handle(Guid id, ISender sender)
    {
        var result = await sender.Send(new Command(id));

        var response = result.Adapt<Response>();

        // TODO return better response
        return (response == null)
            ? Results.BadRequest("Failed to delete Product")
            : Results.NoContent();
    }
}
