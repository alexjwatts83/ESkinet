namespace ESkitNet.API.Exceptions;

public class ProductNotFoundException(Guid Id) : NotFoundException("Product", Id)
{
}

public class CartNotFoundException(string Id) : NotFoundException("Cart", Id)
{
}
