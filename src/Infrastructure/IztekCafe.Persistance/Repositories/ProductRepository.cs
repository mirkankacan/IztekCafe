using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Enums;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product, int>(context), IProductRepository
    {
        public async Task<IEnumerable<Product?>> GetActivesWithCategoryAndStockAsync(CancellationToken cancellationToken)
        {
            return await context.Products
                .AsNoTracking()
                .Where(x => x.Status == ProductStatus.Active)
                .Include(x => x.Category)
                .Include(x => x.Stock)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdWithCategoryAndStockAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Products
                 .AsNoTracking()
                 .Include(x => x.Category)
                 .Include(x => x.Stock)
                 .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Product?>> GetPagedWithCategoryAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var products = await context.Products
             .AsNoTracking()
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .Include(x => x.Category)
             .Include(x => x.Stock)
             .ToListAsync(cancellationToken);
            var count = await context.Products.CountAsync(cancellationToken);
            PagedResult<Product?> pagedResult = new(products, count, pageNumber, pageSize);
            return pagedResult;
        }

        public async Task<IEnumerable<Product?>> GetWithCategoryAndStockAsync(CancellationToken cancellationToken)
        {
            return await context.Products
                .AsNoTracking()
                 .Include(x => x.Category)
                 .Include(x => x.Stock)
                 .ToListAsync(cancellationToken);
        }
    }
}