using IztekCafe.Domain.Entities.Base;

namespace IztekCafe.Domain.Entities
{
    public class Stock : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public bool HasStock => Quantity > 0;
    }
}