using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class OrderRepository(AppDbContext context) : GenericRepository<Order, Guid>(context), IOrderRepository
    {
    }
}