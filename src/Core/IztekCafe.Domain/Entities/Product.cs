using IztekCafe.Domain.Entities.Base;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Domain.Entities
{
    public class Product : BaseEntity<int>
    {
        public Product()
        {
            Status = 0;
        }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ProductStatus Status { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public Stock? Stock { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}