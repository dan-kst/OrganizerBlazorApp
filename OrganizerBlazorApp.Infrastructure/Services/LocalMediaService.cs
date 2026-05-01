using OrganizerBlazorApp.Domain.Interfaces;

namespace OrganizerBlazorApp.Infrastructure.Services;

public class LocalMediaService(string webRootPath) : IMediaService
{
  private readonly string _webRootPath = webRootPath;

  public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
  {
    var uploadPath = Path.Combine(_webRootPath, "uploads");

    if (!Directory.Exists(uploadPath))
      Directory.CreateDirectory(uploadPath);

    var uniqueName = $"{Guid.NewGuid()}_{fileName}";
    var fullPath = Path.Combine(uploadPath, uniqueName);

    await using (var targetStream = new FileStream(fullPath, FileMode.Create))
    {
      await fileStream.CopyToAsync(targetStream);
    }

    return $"/uploads/{uniqueName}";
  }

  public void DeleteFile(string filePath) { }
}