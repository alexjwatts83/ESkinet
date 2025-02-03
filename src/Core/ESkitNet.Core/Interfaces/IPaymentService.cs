namespace ESkitNet.Core.Interfaces;

public interface IPaymentService
{
    Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId, CancellationToken cancellationToken);
    Task<string> RefundPayment(string paymentIntentId);
}
