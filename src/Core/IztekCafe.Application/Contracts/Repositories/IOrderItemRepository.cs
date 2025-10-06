using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem, int>
    {
    }
}