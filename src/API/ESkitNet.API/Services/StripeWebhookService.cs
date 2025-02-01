using ESkitNet.API.Orders.Dtos;
using ESkitNet.API.SignalR;
using ESkitNet.Core.Specifications;
using Microsoft.AspNetCore.SignalR;
using Stripe;

namespace ESkitNet.API.Services;

public class StripeWebhookService(IHttpContextAccessor context, IServiceProvider sp, 
    IHubContext<NotificationHub> hubContext, ILogger<StripeWebhookService> logger)
    : IStripeWebhookService
{
    public Event ConstructStripeEvent(HttpRequest request, string json, string webHookSecret)
    {
        try
        {
            return EventUtility.ConstructEvent(json, request.Headers["Stripe-Signature"], webHookSecret);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "failed to construct Stripe-Signature event");
            throw new StripeException("Invalid Sig");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent, CancellationToken cancellationToken)
    {
        if (intent.Status != "succeeded")
        {
            logger.LogInformation("Status was '{Status}' instead of 'succeeded'", intent.Status);
            return;
        }

        int tryCount = 1;
        // TODO change to publishing an event for later on
        while (tryCount <= 5)
        {
            using var scope = sp.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var spec = new OrderSpecificationForStripe(intent.Id);
            logger.LogDebug("Count {TryCount}", tryCount);
            var order = await unitOfWork.Repository<Order, OrderId>().GetOneWithSpecAsync(spec, cancellationToken);

            if (order == null)
            {
                if (tryCount < 5)
                {
                    tryCount++;
                    logger.LogDebug("Abot to sleep and attempt # {TryCount}", tryCount);
                    Thread.Sleep(1000);
                    logger.LogDebug("continue");
                    continue;
                }
                throw new Exception("Order Not Found with specified intent");
            }

            var totalAsLong = (long)order.Total() * 100;

            logger.LogInformation("Order Total {OrderTotal}, Intent Total {IntentTotal}", totalAsLong, intent.Amount);

            if (totalAsLong != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMismatch;
                logger.LogWarning("Order Total didn't match Intent total, {OrderTotal} != {IntentTotal}", totalAsLong, intent.Amount);
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
            }

            await unitOfWork.Complete(cancellationToken);

            var connectionId = NotificationHub.GetConnectionStringByEmail(order.BuyerEmail);

            logger.LogInformation($"connectionId = {connectionId}");

            if (!string.IsNullOrWhiteSpace(connectionId))
            {
                logger.LogInformation($"Sending Message");
                await hubContext.Clients.Client(connectionId).SendAsync("OrderCompleteNotification", order.Adapt<DisplayOrderDto>());
                logger.LogInformation($"Message Sent");
            } else
            {
                logger.LogWarning("No connection id found for '{BuyerEmail}'", order.BuyerEmail);
            }

            break;
        }
    }

    public async Task<IResult> Process(string webHookSecret, CancellationToken cancellationToken)
    {
        try
        {
            var request = (context?.HttpContext?.Request)
                ?? throw new Exception("For some crazy reason the request is null");

            var json = await new StreamReader(request.Body).ReadToEndAsync(cancellationToken);

            var stripeEvent = ConstructStripeEvent(request, json, webHookSecret);
            if (stripeEvent.Data.Object is not PaymentIntent intent)
            {
                return Results.BadRequest("Invalid Event data");
            }

            await HandlePaymentIntentSucceeded(intent, cancellationToken);

            return Results.Ok();
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Stripe Weekhook Error");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpecet error ocurred");
            throw;
        }
    }
}
