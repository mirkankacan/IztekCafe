using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.CategoryDtos
{
    public record UpdateCategoryDto(string Name, CategoryStatus Status);
}