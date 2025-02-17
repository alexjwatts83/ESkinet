﻿namespace ESkitNet.API.Brands.Get;

public record Response(IReadOnlyList<string> Brands);

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/brands", async (ISender sender, string sort = "") =>
        {
            var result = await sender.Send(new Query(sort));

            var response = result.Adapt<Response>();

            return Results.Ok(response.Brands);
        })
        .WithName("GetBrands")
        .Produces<IReadOnlyList<string>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Brands")
        .WithDescription("Get Brands");
    }
}
