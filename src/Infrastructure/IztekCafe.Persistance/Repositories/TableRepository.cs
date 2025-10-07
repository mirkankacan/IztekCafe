using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class TableRepository(AppDbContext context) : GenericRepository<Table, int>(context), ITableRepository
    {
        public async Task<Table?> GetActiveByIdWithOrderAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Tables
                          .AsNoTracking()
                          .Include(x => x.Orders)
                          .ThenInclude(x => x.OrderItems)
                          .ThenInclude(x => x.Product)
                          .Include(x => x.Orders)
                          .ThenInclude(x => x.Payment)
                          .FirstOrDefaultAsync(x => x.Id == id && x.Status == TableStatus.Occupied, cancellationToken);
        }


        public async Task<bool> UpdateStatusAsync(int id, TableStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var table = await context.Tables.FindAsync(id);
                if (table == null) return false;
                table.Status = status;
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}