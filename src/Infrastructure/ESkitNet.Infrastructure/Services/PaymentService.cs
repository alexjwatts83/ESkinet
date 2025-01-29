using ESkitNet.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Product = ESkitNet.Core.Entities.Product;

namespace ESkitNet.Infrastructure.Services;

public class PaymentService(
    IConfiguration config,
    IShoppingCartService cartService,
    IGenericRepository<Product, ProductId> productRepo,
    IGenericRepository<DeliveryMethod, DeliveryMethodId> deliveryRepo,
    ILogger<PaymentService> logger)
    : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId, CancellationToken cancellationToken)
    {
        logger.LogInformation("CreateOrUpdatePaymentIntent cartId = {CartId}", cartId);
        // TODO change this a type class instead later on
        StripeConfiguration.ApiKey = config["StripeSettings.SecretKey"];

        var cart = await cartService.GetAsync(cartId, cancellationToken);

        if (cart == null)
        {
            logger.LogDebug("No cart found for {CartId}, returning null", cartId);
            return null;
        }

        var shippingPrice = 0m;

        if (cart.DeliveryMethodId != null)
        {
            logger.LogDebug("Delivery Method for cart is not null {DeliveryMethodId}", cart.DeliveryMethodId.Value);

            var deliveryMethod = await deliveryRepo.GetByIdAsync(cart.DeliveryMethodId, cancellationToken);

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
            var productItem = await productRepo.GetByIdAsync(productId, cancellationToken);
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
}
