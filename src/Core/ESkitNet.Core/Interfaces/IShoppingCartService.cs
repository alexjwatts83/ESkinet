namespace ESkitNet.Core.Interfaces;

public interface IShoppingCartService
{
    Task<ShoppingCart?> GetAsync(string id);
    Task<ShoppingCart?> SetAsync(ShoppingCart cart);
    Task<bool> DeleteAsync(string id);
}
