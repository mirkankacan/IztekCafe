using IztekCafe.Application.Contracts.Repositories;

namespace IztekCafe.Application.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStockRepository Stocks { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ITableRepository Tables { get; }
        IPaymentRepository Payments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}