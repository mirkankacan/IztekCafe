using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class PaymentRepository(AppDbContext context) : GenericRepository<Payment, Guid>(context), IPaymentRepository
    {
        public async Task<Payment?> GetByIdWithOrder(Guid id, CancellationToken cancellationToken)
        {
            return await context.Payments.AsNoTracking().Include(x => x.Order).ThenInclude(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Payment?>> GetPagedWithOrderAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var payments = await context.Payments
                       .AsNoTracking()
                       .Skip((pageNumber - 1) * pageSize)
                       .Take(pageSize)
                       .Include(x => x.Order)
                       .ThenInclude(x => x.OrderItems)
                       .ToListAsync(cancellationToken);
            var count = await context.Payments.CountAsync(cancellationToken);
            PagedResult<Payment?> pagedResult = new(payments, count, pageNumber, pageSize);
            return pagedResult;
        }

        public async Task<IEnumerable<Payment?>> GetWithOrder(CancellationToken cancellationToken)
        {
            return await context.Payments.AsNoTracking().Include(x => x.Order).ThenInclude(x => x.OrderItems).ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateStatusAsync(Guid id, PaymentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await context.Payments.FindAsync(id);
                if (payment == null) return false;

                payment.Status = status;
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}