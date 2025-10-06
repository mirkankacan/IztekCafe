using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IStockRepository : IGenericRepository<Stock, int>
    {
        Task<PagedResult<Stock?>> GetPagedWithProduct(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task<IEnumerable<Stock?>> GetWithProduct(CancellationToken cancellationToken);

        Task<Stock?> GetByIdWithProduct(int id, CancellationToken cancellationToken);

        Task<bool> IncreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);

        Task<bool> DecreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    }
}