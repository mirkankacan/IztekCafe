using IztekCafe.Domain.Entities.Base;
using IztekCafe.Domain.Enums;
using MassTransit;

namespace IztekCafe.Domain.Entities
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
            Id = NewId.NextGuid();
        }

        public string OrderCode { get; set; } = null!;
        public OrderStatus Status { get; set; }
        public int TableId { get; set; }
        public Table Table { get; set; } = null!;
        public Guid? PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}