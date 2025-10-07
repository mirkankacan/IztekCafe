using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class OrderRepository(AppDbContext context) : GenericRepository<Order, Guid>(context), IOrderRepository
    {
        public async Task<Order?> GetByIdWithItemsPaymentAndTableAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Orders
              .AsNoTracking()
              .Include(x => x.OrderItems)
              .Include(x => x.Table)
              .Include(x => x.Payment)
              .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Order?>> GetPagedWithItemsPaymentAndTableAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var orders = await context.Orders
          .AsNoTracking()
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .Include(x => x.OrderItems)
          .Include(x => x.Table)
          .Include(x => x.Payment)
          .ToListAsync(cancellationToken);
            var count = await context.Products.CountAsync(cancellationToken);
            PagedResult<Order?> pagedResult = new(orders, count, pageNumber, pageSize);
            return pagedResult;
        }

        public async Task<IEnumerable<Order?>> GetWithItemsPaymentAndTableAsync(CancellationToken cancellationToken)
        {
            return await context.Orders
           .AsNoTracking()
           .Include(x => x.OrderItems)
           .Include(x => x.Table)
           .Include(x => x.Payment)
           .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken)
        {
            var order = await context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = status;
            return true;
        }
    }
}