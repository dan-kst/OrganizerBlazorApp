using OrganizerBlazorApp.Domain.Entities;
using OrganizerBlazorApp.Domain.Enums;

namespace OrganizerBlazorApp.Domain.Extentions;

public static class TodoUnitCss
{
  extension(TodoUnit unit)
  {
    /// <summary>
    /// Returns a Bootstrap class name based on the task's deadline proximity.
    /// </summary>
    public string GetDeadlineColorClass()
    {
      if (unit.Status == ProcessStatus.Completed) return "border-success";

      var timeRemaining = unit.Deadline - DateTime.Now;

      if (timeRemaining.TotalSeconds < 0) return "shadow border-danger"; // Overdue
      if (timeRemaining.TotalHours < 24) return "shadow border-warning"; // Less than 24h

      return "border-light";
    }

    /// <summary>
    /// Returns a Bootstrap class name for icon based on the task's current status.
    /// </summary>
    public string GetProcessStatusIconClass()
    {
      return unit.Status == ProcessStatus.Completed
        ? "bi bi-check-circle-fill me-3"
        : unit.Status == ProcessStatus.Failed ? "bi bi-x-circle-fill me-3" : "bi bi-circle me-3";
    }

    /// <summary>
    /// Returns a Bootstrap class name for text based on the task's current status.
    /// </summary>
    public string GetProcessStatusTextClass()
    {
      return unit.Status == ProcessStatus.Completed || unit.Status == ProcessStatus.Failed
        ? "text-decoration-line-through text-muted" : string.Empty;
    }
  }
}