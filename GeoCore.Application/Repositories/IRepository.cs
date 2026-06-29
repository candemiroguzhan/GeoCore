using System.Linq.Expressions;
using GeoCore.Domain.Entities;
using GeoCore.Shared.Models;

namespace GeoCore.Application.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAllAsync(bool asTracking = false, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<PagedResult<T>> GetPagedAsync(PaginationRequest request, Expression<Func<T, bool>>? predicate = null, bool asTracking = false, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Remove(T entity);

    void RemoveRange(IEnumerable<T> entities);
}
