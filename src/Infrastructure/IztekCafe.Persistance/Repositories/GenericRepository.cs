using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Dtos.Common;
using IztekCafe.Domain.Entities.Base;
using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IztekCafe.Persistance.Repositories
{
    public class GenericRepository<T, TId>(AppDbContext context) : IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : struct
    {
        protected AppDbContext Context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();

        public async Task<bool> AnyAsync(string id, CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public async ValueTask<T?> GetByIdAsync(TId id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<T>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var entities = await _dbSet.AsNoTracking().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            var totalCount = await _dbSet.CountAsync(cancellationToken);
            return new PagedResult<T>(entities, totalCount, pageNumber, pageSize);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}