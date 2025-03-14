
namespace Catalog.API.Products.GetProducts
{
    public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductRespose(IEnumerable<Product> Products);
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request,ISender sender) =>
            {
                var query = request.Adapt<GetProductQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductRespose>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get product list")
            .WithDescription("Get product list");
        }
    }
}
