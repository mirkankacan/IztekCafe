using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
        Task<bool> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken);
    }
}