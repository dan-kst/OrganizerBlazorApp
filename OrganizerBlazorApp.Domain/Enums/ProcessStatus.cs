namespace OrganizerBlazorApp.Domain.Enums;

/// <summary>
/// Enum class that determine completion status of the entity.
/// </summary>
public enum ProcessStatus
{
  None,
  Active,
  Completed,
  Failed,
  Rescheduled
}