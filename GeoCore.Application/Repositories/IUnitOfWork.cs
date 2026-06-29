using GeoCore.Domain.Abstractions;
using GeoCore.Domain.Entities;

namespace GeoCore.Application.Repositories;

public interface IUnitOfWork
{
    IRepository<T> Repository<T>() where T : BaseEntity;

    IGeometryRepository<T> GeometryRepository<T>() where T : BaseEntity, IHasGeometry;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
