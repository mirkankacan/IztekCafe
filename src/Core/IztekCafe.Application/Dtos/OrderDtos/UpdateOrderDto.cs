using IztekCafe.Application.Dtos.OrderItemDtos;

namespace IztekCafe.Application.Dtos.OrderDtos
{
    public record UpdateOrderDto(List<OrderItemDto> OrderItems, int TableId, decimal TotalAmount);
}