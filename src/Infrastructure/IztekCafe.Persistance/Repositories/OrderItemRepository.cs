using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class OrderItemRepository(AppDbContext context) : GenericRepository<OrderItem, int>(context), IOrderItemRepository
    {
    }
}