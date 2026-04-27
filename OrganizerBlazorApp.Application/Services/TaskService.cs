using OrganizerBlazorApp.Domain.Enums;
using OrganizerBlazorApp.Domain.Interfaces;

namespace OrganizerBlazorApp.Application.Services;

/// <summary>
/// Orchestrates business rules for tasks, such as automatic failure propagation.
/// </summary>
public class TaskService(ITaskRepository repository)
{
    private readonly ITaskRepository _repository = repository;

    /// <summary>
    /// Updates task status and applies business rules for mandatory subtasks.
    /// </summary>
    public async Task UpdateProcessStatusAsync(Guid taskId, ProcessStatus newStatus)
    {
        var task = await _repository.GetByIdAsync(taskId);
        if (task == null) return;

        task.Status = newStatus;

        // SOLID: Business Rule - If a mandatory subtask fails, the parent fails too
        if (newStatus == ProcessStatus.Failed && task.IsRequired && task.ParentUnitId.HasValue)
        {
            await UpdateProcessStatusAsync(task.ParentUnitId.Value, ProcessStatus.Failed);
        }
    }
}