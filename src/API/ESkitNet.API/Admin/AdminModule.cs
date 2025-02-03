//namespace ESkitNet.API.Admin;

//public class AdminModule : ICarterModule
//{
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        var group = app
//            .MapGroup("/api/admin")
//            .WithTags("Admin Module")
//            .RequireCors("CorsPolicy")
//            .RequireAuthorization(x => x.RequireRole("Admin"));

//        group.MapGet("/orders", GetById.Endpoint.Handle)
//            .WithName("GetCart");
//    }
//}
