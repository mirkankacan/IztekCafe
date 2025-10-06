using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.ProductDtos
{
    public record ProductDetailDto(int Id, string Name, string? Description, ProductStatus Status, string StatusName, decimal Price, int CategoryId, string CategoryName, int? StockQuantity, string? StockUnit, bool? HasStock, DateTime CreatedAt, DateTime? UpdatedAt);
}