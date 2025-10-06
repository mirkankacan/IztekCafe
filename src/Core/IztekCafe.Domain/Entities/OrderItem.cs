using IztekCafe.Domain.Entities.Base;

namespace IztekCafe.Domain.Entities
{
    public class OrderItem : BaseEntity<int>
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}