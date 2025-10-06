using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.OrderDtos
{
    public record OrderDto(Guid Id, string OrderCode, OrderStatus Status, string StatusName, string TableName, PaymentStatus? PaymentStatus, string? PaymentStatusName, decimal TotalAmount);
}