using IztekCafe.Domain.Entities.Base;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Domain.Entities
{
    public class Table : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public TableStatus Status { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}