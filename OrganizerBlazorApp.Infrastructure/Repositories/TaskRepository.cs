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
        .Include(t => t.SubTasks)
        .Include(t => t.Attachments)
        .Where(t => t.ParentUnitId == null)
        .ToListAsync();
  }
  public async Task UpdateAsync(TodoUnit unit)
  {
    _context.Entry(unit).State = EntityState.Modified;
    await _context.SaveChangesAsync();
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