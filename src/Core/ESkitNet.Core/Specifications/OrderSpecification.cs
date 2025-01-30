namespace ESkitNet.Core.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(string email) : base (x => x.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderDate);
    }

    public OrderSpecification(string email, OrderId id) : base(x => x.BuyerEmail == email && x.Id == id)
    {
        AddInclude(nameof(Order.OrderItems));
        AddInclude(nameof(Order.DeliveryMethod));
    }
}