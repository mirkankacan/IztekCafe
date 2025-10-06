using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        Task<IEnumerable<Product?>> GetWithCategoryAndStock(CancellationToken cancellationToken);
        Task<PagedResult<Product?>> GetPagedWithCategory(int pageNumer, int pageSize, CancellationToken cancellationToken);
        Task<Product?> GetByIdWithCategoryAndStock(int id, CancellationToken cancellationToken);
    }
}