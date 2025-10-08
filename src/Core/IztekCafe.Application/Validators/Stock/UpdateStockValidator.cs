using FluentValidation;
using IztekCafe.Application.Dtos.StockDtos;

namespace IztekCafe.Application.Validators.Stock
{
    public class UpdateStockValidator : AbstractValidator<UpdateStockDto>
    {
        public UpdateStockValidator()
        {
            RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Ürün ID'si boş olamaz");

            RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Geçersiz stok miktarı");

            RuleFor(x => x.Unit)
                    .NotEmpty().WithMessage("Birim boş olamaz");
        }
    }
}