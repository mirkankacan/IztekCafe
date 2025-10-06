using IztekCafe.Domain.Entities;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
        Task<Category?> GetByIdWithProductsAsync(int id, CancellationToken cancellationToken);
    }
}