namespace IztekCafe.Application.Dtos.PaymentDtos
{
    public record CreatePaymentDto(decimal Amount, Guid OrderId);
}