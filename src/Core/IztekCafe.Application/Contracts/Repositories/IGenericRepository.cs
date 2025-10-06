using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities.Base;
using System.Linq.Expressions;

namespace IztekCafe.Application.Contracts.Repositories
{
    public interface IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        Task<bool> AnyAsync(string id, CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        ValueTask<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);

        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

        Task<PagedResult<T>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}