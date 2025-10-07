using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<IEnumerable<Product?>> GetWithCategoryAndStockAsync(CancellationToken cancellationToken);

        Task<PagedResult<Product?>> GetPagedWithCategoryAsync(int pageNumer, int pageSize, CancellationToken cancellationToken);

        Task<Product?> GetByIdWithCategoryAndStockAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<Product?>> GetActivesWithCategoryAndStockAsync(CancellationToken cancellationToken);
    }
}