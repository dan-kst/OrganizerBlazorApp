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
        .Include(t => t.SubTasks)
        .Include(t => t.Attachments)
        .FirstOrDefaultAsync(t => t.Id == id);
  }
  public async Task<IEnumerable<TodoUnit>?> GetAllAsync()
  {
    return await _context.TodoUnits.ToListAsync() ?? [];
  }


  public async Task AddAsync(TodoUnit task)
  {
    await _context.TodoUnits.AddAsync(task);
    await _context.SaveChangesAsync();
  }
  public async Task<bool> AreMandatorySubtasksMetAsync(Guid taskId)
  {
    // Logic: Check if any REQUIRED subtask for the given parent is NOT completed
    var failedMandatory = await _context.TodoUnits
        .AnyAsync(t => t.ParentUnitId == taskId
                    && t.IsRequired
                    && t.Status == ProcessStatus.Failed);

    return !failedMandatory;
  }
}