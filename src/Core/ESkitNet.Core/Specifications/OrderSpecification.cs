namespace ESkitNet.Core.Specifications;

public class OrderSpecification : BaseSpecification<Order>
{
    //public OrderSpecification(string email) : base (x => x.BuyerEmail == email)
    //{
    //    AddInclude(o => o.OrderItems);
    //    AddInclude(o => o.DeliveryMethod);
    //    AddOrderByDescending(o => o.OrderDate);
    //}

    //public OrderSpecification(string email, OrderId id) : base(x => x.BuyerEmail == email && x.Id == id)
    //{
    //    AddInclude(nameof(Order.OrderItems));
    //    AddInclude(nameof(Order.DeliveryMethod));
    //}

    public OrderSpecification(OrdersSpecParams specParams) : base(x => 
        string.IsNullOrWhiteSpace(specParams.Status) || x.Status == ParseStatus(specParams.Status)
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageNumber - 1), specParams.PageSize);

        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);

        AddOrderByDescending(o => o.OrderDate);
    }

    public OrderSpecification(OrderSpecParams specParams) : base(x =>
        (string.IsNullOrWhiteSpace(specParams.Email) || x.BuyerEmail == specParams.Email) &&
        (string.IsNullOrWhiteSpace(specParams.PaymentId) || x.PaymentIntentId == specParams.PaymentId) &&
        (specParams.Id == null || x.Id == specParams.Id)
    )
    {
        AddInclude(nameof(Order.OrderItems));
        AddInclude(nameof(Order.DeliveryMethod));
    }

    private static OrderStatus? ParseStatus(string status)
    {
        if (Enum.TryParse<OrderStatus>(status, true, out var enumValue))
        {
            return enumValue;
        }

        return null;
    }
}

//public class OrderSpecificationForStripe : BaseSpecification<Order>
//{
//    public OrderSpecificationForStripe(string paymentIntentId) : base(x => x.PaymentIntentId == paymentIntentId)
//    {
//        AddInclude(nameof(Order.OrderItems));
//        AddInclude(nameof(Order.DeliveryMethod));
//    }
//}