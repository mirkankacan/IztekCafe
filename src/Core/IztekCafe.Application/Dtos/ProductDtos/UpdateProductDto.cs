using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.ProductDtos
{
    public record UpdateProductDto(string Name, string? Description, decimal Price, int CategoryId, ProductStatus Status);
}