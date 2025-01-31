namespace ESkitNet.API.Payments.StripeWebhook;

public static class Endpoint
{
    public static async Task<IResult> Handle(
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
