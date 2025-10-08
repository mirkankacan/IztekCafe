using FluentValidation;
using IztekCafe.Application.Dtos.OrderDtos;
using IztekCafe.Application.Validators.OrderItem;

namespace IztekCafe.Application.Validators.Order
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.TableId)
                .GreaterThan(0).WithMessage("Geçersiz masa numarası");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Sipariş en az bir ürün içermelidir");
            RuleForEach(x => x.OrderItems).SetValidator(new CreateOrderItemValidator());
        }
    }
}