using IztekCafe.Application.Dtos.OrderItemDtos;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.TableDtos
{
    public record TableOrderDto(int Id, TableStatus Status, string StatusName, Guid? OrderId, string? OrderCode, OrderStatus? OrderStatus, string? OrderStatusName, decimal? TotalAmount, List<OrderItemDto>? OrderItems, Guid? PaymentId, PaymentStatus? PaymentStatus, string? PaymentStatusName, DateTime? OrderCreatedAt);
}