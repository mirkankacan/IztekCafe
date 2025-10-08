using FluentValidation;
using IztekCafe.Application.Dtos.OrderItemDtos;

namespace IztekCafe.Application.Validators.OrderItem
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Geçersiz ürün ID'si");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar 0'dan büyük olmalıdır");
        }
    }
}