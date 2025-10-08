using FluentValidation;
using IztekCafe.Application.Dtos.TableDtos;

namespace IztekCafe.Application.Validators.Table
{
    public class UpdateTableValidator : AbstractValidator<UpdateTableDto>
    {
        public UpdateTableValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Masa adı boş olamaz")
                .MaximumLength(20).WithMessage("Masa adı en fazla 20 karakter olabilir");
        }
    }
}