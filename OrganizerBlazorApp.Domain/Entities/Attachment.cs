using OrganizerBlazorApp.Domain.Common;
using OrganizerBlazorApp.Domain.Enums;
namespace OrganizerBlazorApp.Domain.Entities;

/// <summary>
/// Represent media, attached to the task entity.
/// </summary>
public class Attachment : BaseEntity
{
  /// <summary>A name of the file.</summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>A relative path of the file.</summary>
  public string FilePath { get; set; } = string.Empty;

  /// <summary>An extention of the file.</summary>
  public string Extension { get; set; } = string.Empty;

  /// <summary>A category of file.</summary>
  public MediaType Type { get; set; }
  /// <summary>An id of parent unit.</summary>
  public Guid ParentUnitId { get; set; }
  /// <summary>A reference to parent unit.</summary>
  public virtual TodoUnit ParentUnit { get; set; } = null!;
  /// <summary>
  /// Perform a shallow copy of an class instance.
  /// </summary>
  public Attachment Clone()
  {
    return (Attachment)this.MemberwiseClone();
  }
}