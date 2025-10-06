using IztekCafe.Application.Dtos.OrderItemDtos;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.OrderDtos
{
    public record OrderDetailDto(Guid Id, string OrderCode, OrderStatus Status, string StatusName, int TableId, string TableName, Guid? PaymentId, PaymentStatus? PaymentStatus, string? PaymentStatusName, decimal TotalAmount, List<OrderItemDto> OrderItems, DateTime CreatedAt, DateTime? UpdatedAt);
}