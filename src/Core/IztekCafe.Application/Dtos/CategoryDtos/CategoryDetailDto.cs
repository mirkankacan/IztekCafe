using IztekCafe.Application.Dtos.ProductDtos;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.CategoryDtos
{
    public record class CategoryDetailDto(int Id, string Name, CategoryStatus Status, string StatusName, DateTime CreatedAt, DateTime? UpdatedAt, List<ProductDto> Products);
}