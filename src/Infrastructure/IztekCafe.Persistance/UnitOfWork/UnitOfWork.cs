using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Persistance.Data.Context;
using IztekCafe.Persistance.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace IztekCafe.Persistance.UnitOfWork
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
    {
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;
        private IStockRepository? _stocks;
        private IOrderRepository? _orders;
        private IOrderItemRepository? _orderItems;
        private IProductRepository? _products;
        private ICategoryRepository? _categories;
        private ITableRepository? _tables;
        private IPaymentRepository? _payments;

        public IStockRepository Stocks =>
         _stocks ??= new StockRepository(context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(context);

        public IOrderItemRepository OrderItems =>
            _orderItems ??= new OrderItemRepository(context);

        public IProductRepository Products =>
            _products ??= new ProductRepository(context);

        public ICategoryRepository Categories =>
            _categories ??= new CategoryRepository(context);

        public ITableRepository Tables =>
            _tables ??= new TableRepository(context);

        public IPaymentRepository Payments =>
            _payments ??= new PaymentRepository(context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _transaction?.CommitAsync(cancellationToken);
            }
            catch
            {
                await _transaction?.RollbackAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _transaction?.RollbackAsync(cancellationToken);
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                context?.Dispose();
                _disposed = true;
            }
        }
    }
}