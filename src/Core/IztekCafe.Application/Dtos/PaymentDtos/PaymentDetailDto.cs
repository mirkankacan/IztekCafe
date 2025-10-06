using IztekCafe.Application.Dtos.OrderItemDtos;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.PaymentDtos
{
    public record PaymentDetailDto(Guid Id, PaymentStatus Status, string StatusName, decimal Amount, DateTime CreatedAt, Guid OrderId, string OrderCode, List<OrderItemDto> OrderItems);
}