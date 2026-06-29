using GeoCore.Application.Repositories;
using GeoCore.Domain.Abstractions;
using GeoCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GeoCore.Infrastructure.Repositories;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly Dictionary<Type, object> _repositories = [];
    private readonly Dictionary<Type, object> _geometryRepositories = [];

    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);

        if (!_repositories.TryGetValue(type, out var repository))
        {
            repository = new EfRepository<T>(_dbContext);
            _repositories[type] = repository;
        }

        return (IRepository<T>)repository;
    }

    public IGeometryRepository<T> GeometryRepository<T>() where T : BaseEntity, IHasGeometry
    {
        var type = typeof(T);

        if (!_geometryRepositories.TryGetValue(type, out var repository))
        {
            repository = new EfGeometryRepository<T>(_dbContext);
            _geometryRepositories[type] = repository;
        }

        return (IGeometryRepository<T>)repository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
