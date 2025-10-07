using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order, Guid>
    {
        Task<IEnumerable<Order?>> GetWithItemsPaymentAndTableAsync(CancellationToken cancellationToken);
        Task<PagedResult<Order?>> GetPagedWithItemsPaymentAndTableAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<Order?> GetByIdWithItemsPaymentAndTableAsync(Guid id, CancellationToken cancellationToken);

        Task<bool> UpdateStatusAsync(Guid id, OrderStatus status, CancellationToken cancellationToken);
    }
}