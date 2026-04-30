using OrganizerBlazorApp.Domain.Entities;
using OrganizerBlazorApp.Domain.Enums;

namespace OrganizerBlazorApp.Domain.Extentions;

public static class TodoUnitCss
{
  extension(TodoUnit unit)
  {
    /// <summary>
    /// Returns a CSS class name based on the task's deadline proximity.
    /// </summary>
    public string GetDeadlineColorClass()
    {
      if (unit.Status == ProcessStatus.Completed) return "border-success";

      var timeRemaining = unit.Deadline - DateTime.Now;

      if (timeRemaining.TotalSeconds < 0) return "border-danger bg-danger bg-gradient"; // Overdue
      if (timeRemaining.TotalHours < 24) return "border-warning bg-warning bg-gradient"; // Less than 24h

      return "border-light";
    }
  }
}