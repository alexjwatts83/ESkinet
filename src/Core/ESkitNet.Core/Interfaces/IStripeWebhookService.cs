using Microsoft.AspNetCore.Http;

namespace ESkitNet.Core.Interfaces;

// TODO move this later on when all is working
public interface IStripeWebhookService
{
    Task<IResult> Process(string webHookSecret, CancellationToken cancellationToken);
}
