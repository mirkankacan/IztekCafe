using IztekCafe.Domain.Entities.Base;
using IztekCafe.Domain.Enums;
using MassTransit;

namespace IztekCafe.Domain.Entities
{
    public class Payment : BaseEntity<Guid>
    {
        public Payment()
        {
            Id = NewId.NextGuid();
        }

        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}