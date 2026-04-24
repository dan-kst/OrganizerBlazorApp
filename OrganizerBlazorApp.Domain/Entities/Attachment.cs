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
}