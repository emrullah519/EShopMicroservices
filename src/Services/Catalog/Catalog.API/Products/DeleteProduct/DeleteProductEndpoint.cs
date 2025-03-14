
namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest();
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{guid}",
                async (Guid guid, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteProductCommand(guid));
                    return Results.Ok(result.Adapt<DeleteProductResponse>());
                })
                .WithName("Delete product")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("delete product")
                .WithDescription("delete product api which takes id as guid as parameter");
        }
    }
}
