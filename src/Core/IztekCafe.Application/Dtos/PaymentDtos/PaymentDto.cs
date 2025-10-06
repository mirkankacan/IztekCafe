using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Dtos.PaymentDtos
{
    public record PaymentDto(Guid Id, PaymentStatus Status, string StatusName, decimal Amount, Guid OrderId, string OrderCode, DateTime CreatedAt);
}