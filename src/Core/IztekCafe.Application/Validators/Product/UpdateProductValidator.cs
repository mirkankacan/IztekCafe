using FluentValidation;
using IztekCafe.Application.Dtos.ProductDtos;

namespace IztekCafe.Application.Validators.Product
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz")
                .MaximumLength(100).WithMessage("Ürün adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Description)
                .MaximumLength(300).WithMessage("Açıklama en fazla 300 karakter olabilir")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Fiyat boş olamaz")
                .GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori Id boş olamaz")
                .GreaterThan(0).WithMessage("Kategori Id 0'dan büyük olmalıdır");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz ürün durumu");
        }
    }
}