using FluentValidation;
using IztekCafe.Application.Dtos.PaymentDtos;

namespace IztekCafe.Application.Validators.Payment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Geçersiz ödeme tutarı");
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Sipariş ID'si boş olamaz");
        }
    }
}