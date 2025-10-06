namespace IztekCafe.Application.Dtos.OrderItemDtos
{
    public record OrderItemDto(int Id, int Quantity, decimal Price, decimal TotalPrice, int ProductId, string ProductName);
}