
using FluentValidation;
using FluentValidation.Validators;

namespace Order.Application.Commands.OrderCreate
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateValidator()
        {
            RuleFor(o => o.SellerUserName)
                .EmailAddress()
                .NotEmpty();
            RuleFor(o => o.ProductId)
                .NotEmpty();
        }
    }
}
