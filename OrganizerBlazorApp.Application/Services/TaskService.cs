using OrganizerBlazorApp.Domain.Entities;
using OrganizerBlazorApp.Domain.Enums;
using OrganizerBlazorApp.Domain.Interfaces;

namespace OrganizerBlazorApp.Application.Services;

/// <summary>
/// Orchestrates business rules for units, such as automatic failure propagation.
/// </summary>
public class TaskService(ITaskRepository repository)
{
  private readonly ITaskRepository _repository = repository;

  /// <summary>
  /// Creates a new unit and persists it to the database.
  /// </summary>
  /// <param name="unit">The unit entity to create.</param>
  public async Task CreateTaskAsync(TodoUnit unit)
  {
    if (string.IsNullOrWhiteSpace(unit.Title))
      throw new ArgumentException("Task title cannot be empty.");

    await _repository.AddAsync(unit);
  }

  /// <summary>
  /// Helper method to seed a sample task with subtasks for testing.
  /// </summary>
  public async Task SeedSampleTaskAsync()
  {
    var parentUnit = new TodoUnit
    {
      Title = "Release Business Organizer MVP",
      Description = "Initial project launch for feedback",
      Deadline = DateTime.Now.AddDays(7),
      Status = ProcessStatus.Active
    };

    parentUnit.SubTasks.Add(new TodoUnit
    {
      Title = "Setup SQLite Database",
      IsRequired = true,
      Deadline = DateTime.Now.AddDays(1)
    });

    parentUnit.SubTasks.Add(new TodoUnit
    {
      Title = "Design Multimedia UI",
      IsRequired = false,
      Deadline = DateTime.Now.AddDays(3)
    });

    await _repository.AddAsync(parentUnit);
  }

  /// <summary>
  /// Delete a specified unit and remove it from the database.
  /// </summary>
  /// <param name="id">An id of the unit entity to delete.</param>
  public async Task DeleteTaskAsync(Guid id)
  {
    await _repository.DeleteAsync(id);
  }

  /// <summary>
  /// Updates unit status and applies business rules for mandatory subunits.
  /// </summary>
  public async Task UpdateProcessStatusAsync(Guid unitId, ProcessStatus newStatus)
  {
    var unit = await _repository.GetByIdAsync(unitId);
    if (unit == null) return;

    unit.Status = newStatus;

    // If a mandatory subunit fails, the parent fails too
    if (newStatus == ProcessStatus.Failed && unit.IsRequired && unit.ParentUnitId.HasValue)
    {
      await UpdateProcessStatusAsync(unit.ParentUnitId.Value, ProcessStatus.Failed);
    }
  }

  /// <summary>
  /// Toogle progress status property of unit.
  /// </summary>
  /// <param name="unitId">An id of the unit entity to toggle its property.</param>
  public async Task ToggleProcessStatusAsync(Guid unitId)
  {
    var task = await _repository.GetByIdAsync(unitId);
    if (task == null) return;

    var newStatus = task.Status == ProcessStatus.Active
        ? ProcessStatus.Completed
        : ProcessStatus.Active;

    await UpdateProcessStatusAsync(unitId, newStatus);
  }
}