using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class TableRepository(AppDbContext context) : GenericRepository<Table, int>(context), ITableRepository
    {
        public async Task<bool> UpdateStatusAsync(int id, TableStatus status, CancellationToken cancellationToken)
        {
            var table = await context.Tables.FindAsync(id);
            if (table == null) return false;

            table.Status = status;
            return true;
        }
    }
}