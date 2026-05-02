using OrganizerBlazorApp.Domain.Common;
using OrganizerBlazorApp.Domain.Enums;

namespace OrganizerBlazorApp.Domain.Entities;

/// <summary>
/// Represents a task or event within the organizer.
/// Implements parent-child relationships for complex task management.
/// </summary>
public class TodoUnit : BaseEntity
{
  /// <summary>
  /// Gets or sets the title of the unit.
  /// </summary>
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets the description of the unit.
  /// </summary>
  public string? Description { get; set; }

  /// <summary>
  /// Gets or sets the unit's deadline.
  /// Logic triggers after this date to prompt user action.
  /// </summary>
  public DateTime Deadline { get; set; }

  /// <summary>
  /// Gets or sets the unit's completion process status.
  /// Logic triggers after changing status.
  /// </summary>
  public ProcessStatus Status { get; set; } = ProcessStatus.Active;

  /// <summary>
  /// Indicates whether the unit is mandatory for its parent's success.
  /// </summary>
  public bool IsRequired { get; set; } = true;

  /// <summary>
  /// Gets or sets id of parent unit it depends on.
  /// </summary>
  public Guid? ParentUnitId { get; set; }

  /// <summary>
  /// Gets or sets reference of parent unit it depends on.
  /// </summary>
  public virtual TodoUnit? ParentUnit { get; set; }

  /// <summary>
  /// Navigation property for subtasks.
  /// </summary>
  public virtual ICollection<TodoUnit> SubTasks { get; set; } = [];

  /// <summary>
  /// Navigation property for mediafile attachments.
  /// </summary>
  public virtual ICollection<Attachment> Attachments { get; set; } = [];

  /// <summary>
  /// Perform a shallow copy of an class instance.
  /// </summary>
  public void Clone(TodoUnit newUnit)
  {
    this.Id = newUnit.Id;
    this.Title = newUnit.Title;
    this.Description = newUnit.Description;
    this.Deadline = newUnit.Deadline;
    this.Status = newUnit.Status;
    this.IsRequired = newUnit.IsRequired;
    this.ParentUnitId = newUnit.ParentUnitId;
    this.SubTasks = [.. newUnit.SubTasks];
    this.Attachments = [.. newUnit.Attachments];
  }
}