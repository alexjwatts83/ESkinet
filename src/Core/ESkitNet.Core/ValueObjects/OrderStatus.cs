namespace ESkitNet.Core.ValueObjects;

public enum OrderStatus
{
    Pending = 0,
    PaymentReceived = 1,
    PaymentFailed = 2,
    PaymentMismatch = 3,
    Refunded = 4
}
