
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductCommandResult>;
    public record DeleteProductCommandResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(p=>p.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }

    internal class DeleteProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
    {
        public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id)
                ?? throw new ProductNotFoundException(command.Id);
            session.Delete(product);
            await session.SaveChangesAsync();
            return new DeleteProductCommandResult(true);
        }
    }
}
