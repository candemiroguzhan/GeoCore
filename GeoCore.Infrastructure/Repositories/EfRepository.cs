using System.Linq.Expressions;
using GeoCore.Application.Repositories;
using GeoCore.Domain.Entities;
using GeoCore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public EfRepository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        return await Query(asTracking).Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<T>> GetPagedAsync(PaginationRequest request, Expression<Func<T, bool>>? predicate = null, bool asTracking = false, CancellationToken cancellationToken = default)
    {
        var query = Query(asTracking);

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(entity => entity.Id)
            .Skip(request.Skip)
            .Take(request.NormalizedPageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, request.NormalizedPageNumber, request.NormalizedPageSize, total);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return predicate is null
            ? await DbSet.CountAsync(cancellationToken)
            : await DbSet.CountAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await DbSet.AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    protected IQueryable<T> Query(bool asTracking)
    {
        return asTracking ? DbSet : DbSet.AsNoTracking();
    }
}
