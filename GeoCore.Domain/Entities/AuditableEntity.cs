namespace GeoCore.Domain.Entities;

/// <summary>
/// Base entity for records that carry lifecycle timestamps.
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedAt { get; set; }
}
