using Microsoft.EntityFrameworkCore;

using OrganizerBlazorApp.Domain.Entities;
using OrganizerBlazorApp.Domain.Enums;
using OrganizerBlazorApp.Domain.Interfaces;
using OrganizerBlazorApp.Infrastructure.Data;

namespace OrganizerBlazorApp.Infrastructure.Repositories;

public class TaskRepository(AppDbContext context) : ITaskRepository
{
  private readonly AppDbContext _context = context;

  public async Task<TodoUnit?> GetByIdAsync(Guid id)
  {
    return await _context.TodoUnits
        .Include(u => u.SubTasks)
        .Include(u => u.Attachments)
        .FirstOrDefaultAsync(u => u.Id == id);
  }
  public async Task<IEnumerable<TodoUnit>?> GetAllAsync()
  {
    return await _context.TodoUnits
        .Include(u => u.SubTasks)
        .Include(u => u.Attachments)
        .Where(u => u.ParentUnitId == null)
        .ToListAsync();
  }
  /// <summary>
  /// Update simple fields of an entry in database.
  /// Add or remove attachments.
  /// <param name="updatedUnit">A reference to updated unit.</param>
  /// </summary>
  public async Task UpdateAsync(TodoUnit updatedUnit)
  {
    var existingUnit = await _context.TodoUnits
      .Include(u => u.Attachments)
      .Include(u => u.SubTasks)
      .FirstOrDefaultAsync(u => u.Id == updatedUnit.Id);

    if (existingUnit == null) return;

    // 1. Simple fields
    _context.Entry(existingUnit).CurrentValues.SetValues(updatedUnit);

    // 2. Attachment logics
    //Remove
    var remainingAttachmentsIds = updatedUnit.Attachments
      .Select(a => a.Id)
      .ToList();
    var attachmentsToRemove = _context.Attachments
      .Where(dbA => !remainingAttachmentsIds.Contains(dbA.Id))
      .ToList();
    foreach (var attachment in attachmentsToRemove)
    {
      _context.Attachments.Remove(attachment);
    }

    foreach (var incomingAttachment in updatedUnit.Attachments)
    {
      var existingAttachment = existingUnit.Attachments
          .FirstOrDefault(a => a.Id == incomingAttachment.Id);
      if (existingAttachment == null)
      {
        // Add
        existingUnit.Attachments.Add(incomingAttachment);
        _context.Entry(incomingAttachment).State = EntityState.Added;
      }
      else
      {
        _context.Entry(existingAttachment).CurrentValues.SetValues(incomingAttachment);
      }
    }

    // 3. Subtasks logic
    // Remove
    var remainingSubtasksIds = updatedUnit.SubTasks
      .Select(uS => uS.Id)
      .ToList();
    var subtasksToRemove = existingUnit.SubTasks
      .Where(e => !remainingSubtasksIds.Contains(e.Id))
      .ToList();
    foreach (var s in subtasksToRemove)
    {
      existingUnit.SubTasks.Remove(s);
    }

    foreach (var unitSubtask in updatedUnit.SubTasks)
    {
      var subtasks = existingUnit.SubTasks.FirstOrDefault(s => s.Id == unitSubtask.Id);
      if (subtasks == null)
      {
        // Add
        existingUnit.SubTasks.Add(unitSubtask);
        _context.Entry(unitSubtask).State = EntityState.Added;

      }
      else
      {
        _context.Entry(subtasks).CurrentValues.SetValues(unitSubtask);
      }
    }
    await _context.SaveChangesAsync();
  }
  public async Task DeleteAsync(Guid id)
  {
    var unit = await _context.TodoUnits.FindAsync(id);
    if (unit != null)
    {
      _context.TodoUnits.Remove(unit);
      await _context.SaveChangesAsync();
    }
  }


  public async Task AddAsync(TodoUnit unit)
  {
    await _context.TodoUnits.AddAsync(unit);
    await _context.SaveChangesAsync();
  }
  public async Task<bool> AreMandatorySubtasksMetAsync(Guid taskId)
  {
    var failedMandatory = await _context.TodoUnits
        .AnyAsync(u => u.ParentUnitId == taskId
                    && u.IsRequired
                    && u.Status == ProcessStatus.Failed);

    return !failedMandatory;
  }
}