using System.Linq.Expressions;

namespace FileGateway.Domain.Abstractions;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    public Task<IEnumerable<TEntity>> GetAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    public Task<TEntity?> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default);

    public Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default);
}
