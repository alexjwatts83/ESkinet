namespace ESkitNet.Core.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    public OrderSpecification(string email) : base (x => x.BuyerEmail == email)
    {
        
    }

    public OrderSpecification(string email, Guid id) : base(x => x.BuyerEmail == email && x.Id.Value == id)
    {

    }
}