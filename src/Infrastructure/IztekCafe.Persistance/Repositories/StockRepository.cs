using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace IztekCafe.Persistance.Repositories
{
    public class StockRepository(AppDbContext context) : GenericRepository<Stock, int>(context), IStockRepository
    {
        public async Task<bool> DecreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
        {
            try
            {
                var stock = await context.Stocks.FindAsync(productId);
                if (stock == null || stock.Quantity < quantity)
                    return false;

                stock.Quantity -= quantity;
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Stock?> GetByIdWithProductAsync(int id, CancellationToken cancellationToken)
        {
            return await context.Stocks
                 .AsNoTracking()
                 .Include(x => x.Product)
                 .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Stock?>> GetPagedWithProductAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var stocks = await context.Stocks
              .AsNoTracking()
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .Include(x => x.Product)
              .ToListAsync(cancellationToken);
            var count = await context.Products.CountAsync(cancellationToken);
            PagedResult<Stock> pagedResult = new(stocks, count, pageNumber, pageSize);
            return pagedResult;
        }

        public async Task<IEnumerable<Stock?>> GetWithProductAsync(CancellationToken cancellationToken)
        {
            return await context.Stocks
               .AsNoTracking()
                .Include(x => x.Product)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IncreaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
        {
            try
            {
                var stock = await context.Stocks.FindAsync(productId);
                if (stock == null) return false;

                stock.Quantity += quantity;
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}