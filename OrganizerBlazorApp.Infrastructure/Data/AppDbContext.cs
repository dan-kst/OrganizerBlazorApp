using Microsoft.EntityFrameworkCore;

using OrganizerBlazorApp.Domain.Common;
using OrganizerBlazorApp.Domain.Entities;

namespace OrganizerBlazorApp.Infrastructure.Data;

/// <summary>
/// The primary database context for the application.
/// Manages TodoUnits and their associated Attachments.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<TodoUnit> TodoUnits => Set<TodoUnit>();
  public DbSet<Attachment> Attachments => Set<Attachment>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Configure the self-referencing relationship for hierarchical tasks
    modelBuilder.Entity<TodoUnit>()
        .HasMany(t => t.SubTasks)
        .WithOne(t => t.ParentUnit)
        .HasForeignKey(t => t.ParentUnitId)
        .OnDelete(DeleteBehavior.Cascade); // If parent is deleted, subtasks are too

    // Configure the relationship between Task and Attachments
    modelBuilder.Entity<TodoUnit>()
        .HasMany(t => t.Attachments)
        .WithOne(a => a.ParentUnit)
        .HasForeignKey(a => a.ParentUnitId)
        .OnDelete(DeleteBehavior.Cascade);

    // Optional: Seed initial data or configure table names
    modelBuilder.Entity<TodoUnit>().ToTable("TodoUnits");
    modelBuilder.Entity<Attachment>().ToTable("Attachments");
  }

  /// <summary>
  /// Overriding SaveChanges to automatically update timestamps.
  /// </summary>
  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var entries = ChangeTracker
        .Entries()
        .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

    foreach (var entityEntry in entries)
    {
      ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

      if (entityEntry.State == EntityState.Added)
      {
        ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}