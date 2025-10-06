using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
    }
}