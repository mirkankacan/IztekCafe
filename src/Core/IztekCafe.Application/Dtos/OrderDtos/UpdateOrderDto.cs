using IztekCafe.Application.Dtos.OrderItemDtos;

namespace IztekCafe.Application.Dtos.OrderDtos
{
    public record UpdateOrderDto(List<CreateOrderItemDto> OrderItems, int TableId);
}