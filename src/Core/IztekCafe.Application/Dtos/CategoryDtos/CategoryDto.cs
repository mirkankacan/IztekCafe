using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.CategoryDtos
{
    public record CategoryDto(int Id, string Name, CategoryStatus Status, string StatusName);
}