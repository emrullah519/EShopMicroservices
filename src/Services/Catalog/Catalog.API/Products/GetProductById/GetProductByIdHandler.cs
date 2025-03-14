namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResult(Product Product);
    public record GetProductByIdQuery(Guid Guid) : IQuery<GetProductByIdResult>;
    internal class GetProductByIdQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(query.Guid, cancellationToken)
                ?? throw new ProductNotFoundException(query.Guid);
            return new GetProductByIdResult(product);
        }
    }
}
