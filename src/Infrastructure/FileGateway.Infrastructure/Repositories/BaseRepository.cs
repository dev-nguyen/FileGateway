using FileGateway.Domain;
using FileGateway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FileGateway.Infrastructure
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(AppDbContext context)
        {
            _dbSet = Ensure.IsNotNull(context).Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default)
        {
            var existEntity = await GetByIdAsync(id, cancellationToken);
            if (existEntity is not null)
            {
                _dbSet.Update(entity);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default)
        {
            if (predicate is not null)
            {
                return await _dbSet.CountAsync(predicate, cancellationToken);
            }
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            int skip = (pageIndex - 1) * pageSize;
            var query = _dbSet
                .Skip(skip)
                .Take(pageSize);
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync([id], cancellationToken);
            return entity;
        }
    }
}
