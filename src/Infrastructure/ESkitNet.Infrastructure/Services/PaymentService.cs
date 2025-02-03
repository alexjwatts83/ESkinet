using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Product = ESkitNet.Core.Entities.Product;

namespace ESkitNet.Infrastructure.Services;

public class PaymentService(IShoppingCartService cartService, IUnitOfWork unitOfWork, ILogger<PaymentService> logger)
    : IPaymentService
{

    public PaymentService(IConfiguration config, IShoppingCartService cartService, IUnitOfWork unitOfWork, ILogger<PaymentService> logger) 
        : this(cartService, unitOfWork, logger)
    {
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];
    }

    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateOrUpdatePaymentIntent cartId = {CartId}", cartId);

        var cart = await cartService.GetAsync(cartId, cancellationToken);

        if (cart == null)
        {
            logger.LogDebug("No cart found for {CartId}, returning null", cartId);
            return null;
        }

        var shippingPrice = 0m;

        if (cart.DeliveryMethodId != null)
        {
            logger.LogDebug("Delivery Method for cart is not null {DeliveryMethodId}", cart.DeliveryMethodId);

            var deliveryMethodId = DeliveryMethodId.Of(cart.DeliveryMethodId.Value);
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, DeliveryMethodId>().GetByIdAsync(deliveryMethodId, cancellationToken);

            if (deliveryMethod == null)
            {
                logger.LogDebug("No deliveryMethod found for {DeliveryMethodId}, returning null", cart.DeliveryMethodId.Value);
                return null;
            }

            shippingPrice = deliveryMethod.Price;
            logger.LogDebug("Shipping Price set to {ShippingPrice}", shippingPrice);
        }

        foreach (var item in cart.Items)
        {
            var productId = ProductId.Of(new Guid(item.ProductId));
            var productItem = await unitOfWork.Repository<Product, ProductId>().GetByIdAsync(productId, cancellationToken);
            if (productItem == null)
            {
                logger.LogDebug("No Product found for {ProductId} ({ProductName}), returning null", productId.Value, item.ProductName);
                return null;
            }

            if (item.Price != productItem.Price)
            {
                logger.LogDebug("Updated Cart Item Price to {ProductItemPrice} from {CartItemPrice}", productItem.Price, item.Price);
                item.Price = productItem.Price;

            }
        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;

        if (string.IsNullOrWhiteSpace(cart.PaymentIntentId))
        {

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingPrice * 100,
                Currency = "aud",
                PaymentMethodTypes = ["card"]
            };

            logger.LogDebug("PaymentIntentId was empty creating Payment Intent in Stripe");
            logger.LogDebug("{Options}", options);

            intent = await service.CreateAsync(options, cancellationToken: cancellationToken);

            logger.LogDebug("PaymentIntentId = {PaymentIntentId}", intent.Id);
            logger.LogDebug("ClientSecret = {ClientSecret}", intent.ClientSecret);

            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingPrice * 100,
            };

            logger.LogDebug("Updating Payment Intent of Id {PaymentIntentId} in Stripe", cart.PaymentIntentId);
            logger.LogDebug("{Options}", options);

            intent = await service.UpdateAsync(cart.PaymentIntentId, options, cancellationToken: cancellationToken);
        }

        await cartService.SetAsync(cart, cancellationToken);

        return cart;
    }

    public async Task<string> RefundPayment(string paymentIntentId)
    {
        logger.LogInformation("RefundPayment paymentIntentId = {PaymentIntentId}", paymentIntentId);

        var refundOptions = new RefundCreateOptions
        {
            PaymentIntent = paymentIntentId,
        };
        var refundService = new RefundService();
        var result = await refundService.CreateAsync(refundOptions);

        return result.Status;
    }
}
