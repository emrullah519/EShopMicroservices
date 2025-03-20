using FluentValidation;

namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public record DeleteOrderResult(bool IsSuccess);
    public record DeleteOrderCommand(Guid Id) : ICommand<DeleteOrderResult>;

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("OrderId is required");
        }
    }
}
