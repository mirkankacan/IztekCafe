using FluentValidation;
using IztekCafe.Application.Dtos.CategoryDtos;

namespace IztekCafe.Application.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz")
                .MaximumLength(100).WithMessage("Kategori adı en fazla 100 karakter olabilir");
        }
    }
}