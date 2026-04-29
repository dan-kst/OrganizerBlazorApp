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
}