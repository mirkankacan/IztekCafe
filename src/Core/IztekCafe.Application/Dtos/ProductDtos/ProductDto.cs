using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.ProductDtos
{
    public record ProductDto(int Id, string Name, string? Description, ProductStatus Status, string StatusName, decimal Price, int CategoryId, string CategoryName, bool? HasStock);
}