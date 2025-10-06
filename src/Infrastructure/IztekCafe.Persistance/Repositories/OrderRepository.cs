using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class OrderRepository(AppDbContext context) : GenericRepository<Order, Guid>(context), IOrderRepository
    {
        public async Task<bool> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken)
        {
            var order = await context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = status;
            return true;
        }
    }
}