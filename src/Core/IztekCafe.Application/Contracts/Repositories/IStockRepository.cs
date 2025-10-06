using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IStockRepository : IGenericRepository<Stock, int>
    {
        Task<PagedResult<Stock?>> GetPagedWithProductAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<IEnumerable<Stock?>> GetWithProductAsync(CancellationToken cancellationToken);

        Task<Stock?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken);

        Task<bool> IncreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);

        Task<bool> DecreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    }
}