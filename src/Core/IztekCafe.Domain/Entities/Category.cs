using IztekCafe.Domain.Entities.Base;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Domain.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public CategoryStatus Status { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}