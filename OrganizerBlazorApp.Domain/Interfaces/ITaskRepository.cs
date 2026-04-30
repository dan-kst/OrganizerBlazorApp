using OrganizerBlazorApp.Domain.Entities;

namespace OrganizerBlazorApp.Domain.Interfaces;

/// <summary>
/// Defines the contract for task-related data operations.
/// </summary>
public interface ITaskRepository
{
  Task<IEnumerable<TodoUnit>?> GetAllAsync();
  Task<TodoUnit?> GetByIdAsync(Guid id);
  Task AddAsync(TodoUnit unit);
  Task UpdateAsync(TodoUnit unit);

  /// <summary>
  /// Checks if all mandatory subtasks are completed for a given task.
  /// </summary>
  Task<bool> AreMandatorySubtasksMetAsync(Guid unitId);
}