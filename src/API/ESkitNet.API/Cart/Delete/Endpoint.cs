namespace ESkitNet.API.Cart.Delete;

public static class Endpoint
{
    public record Response(bool IsSuccess);
    public record Command(string Id) : ICommand<Result>;

    public record Result(bool IsSuccess);

    public class Handler(IShoppingCartService cartService) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var result = await cartService.DeleteAsync(command.Id, cancellationToken);

            return new Result(result);
        }
    }

    public static async Task<IResult> Handle(string id, ISender sender)
    {
        var result = await sender.Send(new Command(id));

        var response = result.Adapt<Response>();

        // TODO return better response
        return (response == null)
            ? Results.BadRequest("Failed to delete Cart")
            : Results.NoContent();
    }
}
