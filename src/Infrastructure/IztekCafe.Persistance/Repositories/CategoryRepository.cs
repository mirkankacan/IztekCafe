using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, int>(context), ICategoryRepository
    {
        public async Task<Category?> GetByIdWithProductsAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Categories
                .AsNoTracking()
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}