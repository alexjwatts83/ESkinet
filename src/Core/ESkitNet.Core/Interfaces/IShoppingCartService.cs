namespace ESkitNet.Core.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCart?> GetAsync(string id, CancellationToken cancellationToken);
    Task<ShoppingCart?> SetAsync(ShoppingCart cart, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}
