using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;

namespace IztekCafe.Persistance.Repositories
{
    public class TableRepository(AppDbContext context) : GenericRepository<Table, int>(context), ITableRepository
    {
    }
}