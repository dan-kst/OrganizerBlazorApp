namespace OrganizerBlazorApp.Domain.Interfaces;

public interface IMediaService
{
  /// <summary>
  /// Saves a file to the storage and returns the relative path.
  /// </summary>
  Task<string> UploadFileAsync(Stream fileStream, string fileName);
  void DeleteFile(string filePath);
}