﻿using ESkitNet.Core.Specifications;
using Stripe;

namespace ESkitNet.API.Payments.StripeWebhook;

// TODO move this later on when all is working
public interface IStripeWebhookService
{
    Task<IResult> Process(string webHookSecret, CancellationToken cancellationToken);
}

public class StripeWebhookService(IHttpContextAccessor context, IUnitOfWork unitOfWork, ILogger<StripeWebhookService> logger) : IStripeWebhookService
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
        if (intent.Status == "succeeded")
        {
            var spec = new OrderSpecificationForStripe(intent.Id);
            var order = await unitOfWork.Repository<Order, OrderId>().GetOneWithSpecAsync(spec, cancellationToken)


            if (order == null)
                throw new Exception("Order Not Found with specified intent");

            if ((long)order.Total() * 100 != intent.Amount)
            {
                order.Status = OrderStatus.PaymentMismatch;
            }
            else
            {
                order.Status = OrderStatus.PaymenReceived;
            }

            await unitOfWork.Complete(cancellationToken);

            // TODO signal r
        }
    }

    public async Task<IResult> Process(string webHookSecret, CancellationToken cancellationToken)
    {
        var request = (context?.HttpContext?.Request)
                   ?? throw new Exception("For some crazy reason the request is null");

        var json = await new StreamReader(request.Body).ReadToEndAsync();

        try
        {
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

public static class Endpoint
{
    //private static string whSecret = string.Empty;
    public static async Task<IResult> Handle(
        //IHttpContextAccessor context,
        //IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IStripeWebhookService stripeWebhookService,
        CancellationToken cancellationToken)
    {
        var whSecret = configuration["StripeSettings:WhSecret"];
        if (string.IsNullOrEmpty(whSecret))
            throw new InvalidOperationException("Stripe Client secret found not");

        var result = await stripeWebhookService.Process(whSecret, cancellationToken);
        return result;
    }
}
