using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product, int>(context), IProductRepository
    {
        public async Task<Product?> GetByIdWithCategoryAndStock(int id, CancellationToken cancellationToken)
        {
            return await context.Products
                 .AsNoTracking()
                 .Include(x => x.Category)
                 .Include(x => x.Stock)
                 .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Product?>> GetPagedWithCategory(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var products = await context.Products
             .AsNoTracking()
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .Include(x => x.Category)
             .Include(x => x.Stock)
             .ToListAsync(cancellationToken);
            var count = await context.Products.CountAsync(cancellationToken);
            PagedResult<Product> pagedResult = new(products, count, pageNumber, pageSize);
            return pagedResult;
        }

        public async Task<IEnumerable<Product?>> GetWithCategoryAndStock(CancellationToken cancellationToken)
        {
            return await context.Products
                .AsNoTracking()
                 .Include(x => x.Category)
                 .Include(x => x.Stock)
                 .ToListAsync(cancellationToken);
        }
    }
}