using OrganizerBlazorApp.Domain.Entities;
using OrganizerBlazorApp.Domain.Enums;

namespace OrganizerBlazorApp.Domain.Extentions;

public static class TodoSubtasks
{
  extension(TodoUnit unit)
  {
    /// <summary>
    /// Update base task status based on its subtusks status.
    /// </summary>
    public void UpdateStatusFromSubTasks()
    {
      if (unit.SubTasks.Count == 0) return;

      // If all subtasks are completed -> base task also become completed
      if (unit.SubTasks.All(s => s.Status == ProcessStatus.Completed))
      {
        unit.Status = ProcessStatus.Completed;
      }
      // If added new task or changed status of old ones -> base task become again active
      else if (unit.Status == ProcessStatus.Completed)
      {
        unit.Status = ProcessStatus.Active;
      }
    }
  }
}