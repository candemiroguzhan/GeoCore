namespace GeoCore.Domain.Entities;

/// <summary>
/// Base entity with a Guid primary key.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
