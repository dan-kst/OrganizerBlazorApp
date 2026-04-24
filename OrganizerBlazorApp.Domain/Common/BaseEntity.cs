namespace OrganizerBlazorApp.Domain.Common;

/// <summary>
/// Provides a base class for all entities to ensure consistency and DRY principles.
/// </summary>
public abstract class BaseEntity
{
  /// <summary>Unique Identifier for the entity.</summary>
  public Guid Id { get; set; } = Guid.NewGuid();

  /// <summary>Date of creation an instance of the entity.</summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>Time of last update of the entity from moment of creation.</summary>
  public DateTime? UpdatedAt { get; set; }
}
