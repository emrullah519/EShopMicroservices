
namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler(IApplicationDbContext ctx)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Id);
            var order = await ctx.Orders.FindAsync([orderId], cancellationToken)
                ?? throw new OrderNotFoundException(command.Id);
            ctx.Orders.Remove(order);
            await ctx.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);
        }
    }
}
