using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface ITableRepository : IGenericRepository<Table, int>
    {
        Task<bool> UpdateStatusAsync(int id, TableStatus status, CancellationToken cancellationToken);
    }
}