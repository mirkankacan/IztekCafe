namespace IztekCafe.Application.Dtos.ProductDtos
{
    public record CreateProductDto(string Name, string? Description, int CategoryId, decimal Price);
}