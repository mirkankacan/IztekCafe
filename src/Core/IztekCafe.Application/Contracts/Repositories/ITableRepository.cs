using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface ITableRepository : IGenericRepository<Table, int>
    {
        Task<Table?> GetActiveByIdWithOrderAsync(int id, CancellationToken cancellationToken);

        Task<bool> UpdateStatusAsync(int id, TableStatus status, CancellationToken cancellationToken);
    }
}